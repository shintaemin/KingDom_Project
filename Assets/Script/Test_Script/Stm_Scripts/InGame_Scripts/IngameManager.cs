using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인게임 매니저
/*
 ▶ 할일
  - 플레이어 레벨(stage)데이터 확인하고 해당되는 맵 배치
  - 미션매니저의 SetMiision(맵); 함수 호출하여 맵 던져주기
  - 스포너에게 생성할 맵과 데이터 던져주기

    - 작업자 신태민
*/
#endregion

public class IngameManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private MissionManager _msManager;
    [SerializeField] private SpawnManager _sm;
    [SerializeField] private Map_Registry_SO _mapSO;
    [SerializeField] private FadeSystem _fadeSystem;
    [SerializeField] private Map_Stage _currentMap;
    [SerializeField] private CInGameCanvas _ingameCanvas;

    [SerializeField] private int _mapIndex = 1;
    [SerializeField] private int _currentStageCount;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private float _waitTime = 2.5f;

    [SerializeField] private List<EnemyState> _enemys = new List<EnemyState>();
    [SerializeField] private PlayerState _pState;
    #endregion

    #region 내부 변수
    public event Action<EMissionAnswer> MissionEnd;
    private Coroutine _mapChanegeCo;
    #endregion

    #region 외부 호출 함수
    // 게임 시작시 맵 데이터값 받아와서 맵지정
    public void GameStart()
    {
        if (CPlayerDataManager.Instance != null)
        {
            int playerStage = CPlayerDataManager.Instance.CurrentStage;
            
            playerStage = playerStage >= 21 ? UnityEngine.Random.Range(0,21) : playerStage;
            
            _mapIndex = 1;
            SetMap(playerStage, _mapIndex); 

            if (_ingameCanvas != null)
            {
                _currentStageCount = _mapSO.GetStageCount(playerStage);
                EMissionType type = _currentMap.GetMissionType;
                _ingameCanvas.Init(_currentMap.GetPlayerSpawnPos, playerStage, _currentStageCount);
                _ingameCanvas.Standby(_mapIndex, type);
            }
        }
    }

    public MissionBase GetMission => _msManager.GetMission;
    #endregion

    private void Awake()
    {
        if (_msManager == null)
        {
            _msManager = FindFirstObjectByType<MissionManager>();
        }
        if (_mapSO == null)
        {
            Debug.Log($"[IngameManager] : Map_Registry_SO 없음 맵 지정 불가");
            return;
        }
        if (_sm == null)
        {
            _sm = FindFirstObjectByType<SpawnManager>();
        }
        if (_fadeSystem == null)
        {
            if (SceneLoadManager.Instance != null)
            {
                _fadeSystem = SceneLoadManager.Instance.GetFadeSystem;
                if (_fadeSystem == null)
                {
                    Debug.Log("[IngameManager] : 맵전환 페이드 적용 불가 참조오류");
                }
            }
        }
        if (_ingameCanvas == null)
        {
            _ingameCanvas = FindFirstObjectByType<CInGameCanvas>();
        }

        _enemys.Clear();
    }
    private void Start()
    {
        GameStart();
    }

    private void OnDestroy()
    {
        MissionClear();
    }

    private void SetMap(int stageData, int mapIndex)
    {
        if (_msManager == null || _mapSO == null || _sm == null)
        {
            return;
        }

        Map_Stage map = _mapSO.GetMap(stageData, mapIndex);

        // 맵이 없다면
        if (map == null)
        {
            _currentMap = null;
            return;
        }

        // 구독 진행
        Subscription();
        // 미션에 맵전달
        _msManager.SetMission(map);
        // 스포너에 맵전달
        _sm.SetMap(map);
        _currentMap = _sm.GetCurrentMap;

        _sm.SpawnStart();

        CInGameCamera cam = Camera.main.GetComponent<CInGameCamera>();
        if (cam != null)
        {
            cam.InitSetting(Camera.main, _currentMap.GetLeftPin, _currentMap.GetRightPin, _pState.transform, _enemys);
        }
    }

    private IEnumerator CoChangeMap(int stageData, int mapIndex)
    {
        float time = _fadeTime;
        if (_fadeSystem != null)
        {
            _fadeSystem.SetActiveFade(true);
            _fadeSystem.Fade(0, 1, time);
            yield return new WaitForSeconds(time);
        }

        _sm.MapClear();
        SetMap(stageData, mapIndex);

        if (_ingameCanvas != null)
        {
            EMissionType type = _currentMap.GetMissionType;
            _ingameCanvas.Standby(_mapIndex, type);
        }

        yield return new WaitForSeconds(_waitTime);

        if (_fadeSystem != null)
        {
            _fadeSystem.Fade(1, 0, time);
            yield return new WaitForSeconds(time);
            _fadeSystem.SetActiveFade(false);
        }

        _mapChanegeCo = null;
    }

    #region 미션 클리어 구독
    private void Subscription()
    {
        if (_msManager == null || _sm == null)
        {
            return;
        }

        _msManager.OnMissionClearAnswer += MissionClearCheck;
        _sm.OnSpawn += SpawnCheck;
        Debug.Log("[IngameManager] : 클리어 미션 구독완료!");
    }

    private void MissionClear()
    {
        // 미션 클리어시 바로 미션 구독 취소 및 지정된 맵 비우기
        if (_msManager != null)
        {
            _msManager.OnMissionClearAnswer -= MissionClearCheck;
            _currentMap = null;
        }

        // 스폰 구독 해제
        _sm.OnSpawn -= SpawnCheck;

        for (int i = _enemys.Count - 1; i >= 0; i--)
        {
            if (_enemys[i] == null)
            {
                continue;
            }

            EnemyState go = _enemys[i];

            if (go != null && _msManager.GetMission != null)
            {
                go.OnDead -= _msManager.KillEvent;
                _enemys.RemoveAt(i);
            }
        }

        _enemys.Clear();
    }

    // 미션 클리어시 호출함수
    private void MissionClearCheck(EMissionAnswer answer)
    {
        if (_mapChanegeCo != null)
        {
            StopCoroutine(_mapChanegeCo);
            _mapChanegeCo = null;
        }

        if (answer == EMissionAnswer.Success)
        {
            // 여기서 문을 연다
            DoorOpenAnim doorOpen = FindFirstObjectByType<DoorOpenAnim>();

            if (doorOpen != null)
            {
                // 여기서 GO UI 재생
                _ingameCanvas.SetActiveGoImpact(true);
                doorOpen.PlayOpenAnim();
                Door_StageEnd_Col endCol = doorOpen.transform.GetComponentInChildren<Door_StageEnd_Col>();
                
                // 스테이지 종료 충돌 시점 구독
                if (endCol != null)
                {
                    endCol.OnStageEnd += ChaingedNextMap;
                }
            }

            else
            {
                 // 여기서 성공 UI 띄우고 씬전환 입력 대기
                MissionClear();

                MissionEnd?.Invoke(EMissionAnswer.Success);

                if (CPlayerDataManager.Instance != null)
                {
                    CPlayerDataManager.Instance.CurrentStage += 1;
                }
            }
            
            return;
        }

        if (answer == EMissionAnswer.Fail)
        {
            // 여기서 실패 UI 를 띄우고 씬전환 입력대기
            MissionEnd?.Invoke(EMissionAnswer.Fail);
        }
    }

    private void SpawnCheck(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        if (_msManager.GetMission is Kill_Mission && go.TryGetComponent<EnemyState>(out EnemyState eState))
        {
            _enemys.Add(eState);
            eState.OnDead += _msManager.KillEvent;
            Debug.Log($"[IngameManager] : {_enemys.Count} 구독 완료");
        }

        if (go.TryGetComponent<PlayerState>(out _pState))
        {
            Debug.Log($"[IngameManager] : {_pState.gameObject.name} 구독 완료");
        }
    }

    private void ChaingedNextMap(Door_StageEnd_Col endCol)
    {
        endCol.OnStageEnd -= ChaingedNextMap;

        // GO UI 종료
        _ingameCanvas.SetActiveGoImpact(false);

        Debug.Log("다음맵 찾는중");

        // 다음 맵으로 이동 하기위해 인덱스 변경
        _mapIndex++;
        // 미션 클리어하여 해당 미션 구독 취소
        MissionClear();

        if (CPlayerDataManager.Instance != null)
        {
            int playerStage = CPlayerDataManager.Instance.CurrentStage;
            _mapChanegeCo = StartCoroutine(CoChangeMap(playerStage, _mapIndex));
            Destroy(_pState.gameObject);
        }
    }
    #endregion
}