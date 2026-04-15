using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 미션 매니저
/*
 ▶ 할일
  - 인게임에서 미션에만 사용할 매니저
  - 인게임매니저를 통해 맵을 지정받고 해당 맵의 타입 데이터에따른 미션지정
  - 외부에서 이벤트 받을 수 있도록 활성화된 미션 지정
  - MissionManager.OnMissionClearAnswer += , -= 으로 구독진행

    - 작업자 신태민
*/
#endregion

public enum EMissionAnswer
{
    None,
    Fail,
    Success
}

public class MissionManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private MissionBase _currentMission;
    [SerializeField] private SpawnManager _sm;
    [SerializeField] private PlayerState _pState;

    [SerializeField] private IngameUIManager _gameUIManger;
    #endregion

    #region 내부 변수
    public event Action<EMissionAnswer> OnMissionClearAnswer;
    #endregion

    private void Awake()
    {
        if (_sm == null)
        {
            _sm = FindAnyObjectByType<SpawnManager>();
        }

        if (_gameUIManger == null)
        {
            _gameUIManger = FindAnyObjectByType<IngameUIManager>();
            if (_gameUIManger == null)
            {
                Debug.LogWarning("[MissionManager] : 인게임 UI 매니저 캐싱 실패");
            }
        }
        _sm.OnSpawn += SpawnCheck;
    }

    private void OnDestroy()
    {
        ResetMission();
    }

    #region 미션 클리어 구독
    private void Subscription()
    {
        if (_currentMission == null || _sm == null)
        {
            return;
        }

        _currentMission.OnClearMission += MissionClear;
    }

    // 미션 클리어시 호출 함수
    private void MissionClear()
    {
        Debug.Log($"[MissionManager] : MissionClear Success 너 들어오냐?");
        // 미션 클리어시 바로 해당 미션 클리어 구독 취소
        OnMissionClearAnswer?.Invoke(EMissionAnswer.Success);
        ResetMission();
    }

    private void MissionFail()
    {
        Debug.Log($"[MissionManager] : MissionClear Fail 너 들어오냐?");
        OnMissionClearAnswer?.Invoke(EMissionAnswer.Fail);
        ResetMission();
    }

    private void SpawnCheck(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        if (go.TryGetComponent<PlayerState>(out _pState))
        {
            _pState.OnDead += MissionFail;
        }
    }
    #endregion


    #region 외부 호출 함수
    public void SetMission(Map_Stage map)
    {
        EMissionType type = map.GetMissionType;

        switch (type)
        {
            case EMissionType.Kill:

                // 맵에 적 스폰 위치 가져와서 킬미션 생성자로 던져주기
                int killCount = map.GetEnemyCount;
                _currentMission = new Kill_Mission(killCount);
                // 지정한 미션 시작
                _currentMission.StartMission(); 

                // UI 업데이트
                if (_gameUIManger != null)
                {
                    _gameUIManger.GetKillCountUI(0, killCount);
                }

                if (_currentMission == null)
                {
                    Debug.Log($"[MissionManager] : 킬 미션 지정 실패");
                }
                else
                {
                    Debug.Log($"[MissionManager] : 킬 미션 지정 성공");
                }

                // 구독진행
                Subscription();

                break;
            case EMissionType.Goal:

                _currentMission = new Goal_Mission(1);
                _currentMission.StartMission();

                // UI 업데이트
                if (_gameUIManger != null)
                {
                    _gameUIManger.GetKillCountUI(0, 1);
                }
                if (_currentMission == null)
                {
                    Debug.Log($"[MissionManager] : 골 미션 지정 실패");
                }
                else
                {
                    Debug.Log($"[MissionManager] : 골 미션 지정 성공");
                }
                
                // 구독 진행
                Subscription();
                break;
        }
    }

    // 혹시모를 외부 사용을 위해
    public void ResetMission()
    {
        if (_currentMission == null || _sm == null)
        {
            return;
        }

        // 플레이어 사망 구독 해제
        if (_pState != null)
        {
            _pState.OnDead -= MissionFail;
            _pState = null;
        }

        // 미션 구독 해제
        _currentMission.OnClearMission -= MissionClear;
        // 스폰 구독 해제
        _sm.OnSpawn -= SpawnCheck;
    }

    public void KillEvent()
    {
        if (_gameUIManger == null)
        {
            _gameUIManger = FindAnyObjectByType<IngameUIManager>();
            if (_gameUIManger == null)
            {
                Debug.LogWarning("[MissionManager] : 인게임 UI 매니저 캐싱 실패");
                return;
            }
            else
            {
                Debug.LogWarning("[MissionManager] : 인게임 UI 매니저 캐싱 성공");
            }
        }

        _currentMission.CheckClear();
        int target = _currentMission.GetTargetCount();
        int remain = _currentMission.GetRemainCount();
        _gameUIManger.GetKillCountUI(remain, target);

        if (remain >= target)
        {
            _currentMission = null;
        }
    }

    public void GoalEvent()
    {
        if (_currentMission != null && _currentMission is Goal_Mission)
        {
            _currentMission.CheckClear();

            // UI 업데이트 로직 추가
            int target = _currentMission.GetTargetCount();
            int remain = _currentMission.GetRemainCount();
            _gameUIManger.GetKillCountUI(remain, target);

            if (remain >= target)
            {
                _currentMission = null;
            }
        }
        else
        {
            Debug.LogWarning("[MissionManager] 현재 활성화된 골 미션이 없습니다!");
        }
    }

    // 외부에서 지정된 미션구독을 진행하기위해
    public MissionBase GetMission => _currentMission;
    #endregion
}