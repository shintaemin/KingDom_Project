using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    //[SerializeField] private Player_Data_SO _playerData;
    [SerializeField] private Map_Stage _currentMap;

    [SerializeField] private int _mapIndex = 1;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private float _waitTime = 2.5f;

    [SerializeField] private List<EnemyState> _enemys = new List<EnemyState>();
    [SerializeField] private PlayerState _pState;
    #endregion

    #region 내부 변수
    private Coroutine _mapChanegeCo;
    #endregion

    #region 외부 호출 함수
    // 게임 시작시 맵 데이터값 받아와서 맵지정
    public void GameStart()
    {
        //int stateData = _playerData.GetStageData;

        _mapIndex = 1;
        SetMap(1/*stateData*/, _mapIndex);
    }
    #endregion

    private void Awake()
    {
        if (_msManager == null)
        {
            _msManager = FindAnyObjectByType<MissionManager>();
        }
        if (_mapSO == null)
        {
            Debug.Log($"[IngameManager] : Map_Registry_SO 없음 맵 지정 불가");
            return;
        }
        if (_sm == null)
        {
            _sm = FindAnyObjectByType<SpawnManager>();
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

        _enemys.Clear();
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

            // 여기서 씬종료로 연결
            // 여기서 플레이어 스테이지 레벨 ++
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
        if (_msManager == null)
        {
            return;
        }

        _msManager.OnMissionClearAnswer += ClearCheck;
        _sm.OnSpawn += SpawnCheck;
        Debug.Log("[IngameManager] : 클리어 미션 구독완료!");
    }

    private void MissionClear()
    {
        // 미션 클리어시 바로 미션 구독 취소 및 지정된 맵 비우기
        if (_msManager != null)
        {
            _msManager.OnMissionClearAnswer -= ClearCheck;
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
                go.OnDead -= _msManager.GetMission.CheckClear;
                _enemys.RemoveAt(i);
            }
        }

        _enemys.Clear();
    }

    // 미션 클리어시 호출함수
    private void ClearCheck(EMissionAnswer answer)
    {
        if (_mapChanegeCo != null)
        {
            StopCoroutine(_mapChanegeCo);
            _mapChanegeCo = null;
        }

        if (answer == EMissionAnswer.Success)
        {
            // 다음 맵으로 이동 하기위해 인덱스 변경
            _mapIndex++;
            // 미션 클리어하여 해당 미션 구독 취소
            MissionClear();

            // 데이터 다시확인하고 맵 재지정
            //int stateData = _playerData.GetStageData;

            _mapChanegeCo = StartCoroutine(CoChangeMap(1/*stateData*/, _mapIndex));
            return;
        }

        if (answer == EMissionAnswer.Fail)
        {
            // 여기서 씬전환
        }
    }

    private void SpawnCheck(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        if (go.TryGetComponent<EnemyState>(out EnemyState eState))
        {
            _enemys.Add(eState);
            eState.OnDead += _msManager.GetMission.CheckClear;
            Debug.Log($"[IngameManager] : {_enemys.Count} 구독 완료");
        }

        if (go.TryGetComponent<PlayerState>(out _pState))
        {

        }
    }
    #endregion
}