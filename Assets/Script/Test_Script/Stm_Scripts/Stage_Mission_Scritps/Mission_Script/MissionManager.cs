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

    private void SuSubscription()
    {
        if (_currentMission == null)
        {
            return;
        }

        _currentMission.OnClearMission += MissionClear;
    }

    private void MissionClear()
    {


        _currentMission.OnClearMission -= MissionClear;
    }

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

                int killCount = map.GetEnemyPos;
                _currentMission = new Kill_Mission(killCount);
                _currentMission.StartMission();
                SuSubscription();

                break;
            case EMissionType.Rescue:
                _currentMission = null;
                break;
            case EMissionType.Goal:
                _currentMission = null;
                break;
        }
    }

    public void ResetMission()
    {
        if (_currentMission == null)
        {
            return;
        }

        _currentMission = null;
    }

    public MissionBase GetMission => _currentMission;
    #endregion


}
