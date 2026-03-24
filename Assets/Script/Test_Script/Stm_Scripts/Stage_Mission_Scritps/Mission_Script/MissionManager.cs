using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 미션 매니저
/*
 ▶ 할일
  - 인게임에서 미션에만 사용할 매니저
  - 인게임매니저를 통해 맵을 지정받고 해당 맵의 타입 데이터에따른 미션지정
  - 외부에서 이벤트 받을 수 있도록 활성화된 미션 지정
  - MissionManager.GetMission.OnClearMission += , -= 으로 구독진행
*/
#endregion


public class MissionManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private MissionBase _currentMission;
    #endregion

    #region 미션 클리어 구독
    private void Subscription()
    {
        if (_currentMission == null)
        {
            return;
        }

        _currentMission.OnClearMission += MissionClear;
    }

    // 미션 클리어시 호출 함수
    private void MissionClear()
    {

        // 미션 클리어시 바로 해당 미션 클리어 구독 취소
        if (_currentMission != null)
        {
            _currentMission.OnClearMission -= MissionClear;
            ResetMission();
        }
    }
    #endregion


    #region 외부 호출 함수
    public void SetMission(Map_Stage map)
    {
        if (_currentMission != null)
        {
            return;
        }

        EMissionType type = map.GetMissionType;

        switch (type)
        {
            case EMissionType.Kill:

                // 맵에 적 스폰 위치 가져와서 킬미션 생성자로 던져주기
                int killCount = map.GetEnemyCount;
                _currentMission = new Kill_Mission(killCount);
                // 지정한 미션 시작
                _currentMission.StartMission();
                // 구독진행
                Subscription();

                break;
            case EMissionType.Rescue:
                _currentMission = null;
                break;
            case EMissionType.Goal:
                _currentMission = null;
                break;
        }
    }

    // 혹시모를 외부 사용을 위해
    public void ResetMission()
    {
        if (_currentMission == null)
        {
            return;
        }

        _currentMission = null;
    }

    // 외부에서 지정된 미션구독을 진행하기위해
    public MissionBase GetMission => _currentMission;
    #endregion


}
