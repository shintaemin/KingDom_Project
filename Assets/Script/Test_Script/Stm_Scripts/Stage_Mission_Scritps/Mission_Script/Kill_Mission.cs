using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 킬 미션
/*
 ▶ 할일
  - 킬 미션을 정의
  - 생성자 매개변수를 통해 처치해야할 대상의 갯수를 지정
  - 목표 킬 갯수와 현재 킬 갯수가 같아지면 ClearMission() 을 호출 하여 MissionBase 에 이벤트 발행
*/
#endregion


public class Kill_Mission : MissionBase
{
    #region 인스펙터
    [SerializeField] private int _targetKillCount;
    [SerializeField] private int _remainKillCount;
    #endregion

    public Kill_Mission(int count)
    {
        ResetData();
        _targetKillCount = count;
    }

    private void ResetData()
    {
        // 구독 해제
        _targetKillCount = 0;
        _remainKillCount = 0;
    }

    #region 외부 호출 함수
    public override void StartMission()
    {
        // 구독 진행

        Debug.Log($"[Kill_Mission] : 타겟갯수 : {_targetKillCount}");
    }

    public override void CheckClear()
    {
        _remainKillCount++;

        Debug.Log($"[Kill_Mission] : 처치 횟수 : {_remainKillCount}");

        if (_remainKillCount == _targetKillCount)
        {
            ClearMission();
        }
    }

    public override void ClearMission()
    {
        base.ClearMission();
        ResetData();
    }
    #endregion
}
