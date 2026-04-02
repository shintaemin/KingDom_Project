using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 미션 베이스
/*
 ▶ 할일
  - 상속을 활용한 Mission 들의 베이스 스크립트
  - 클리어시 자식에서 ClearMission() 을 호출하여 이벤트 발행

    - 작업자 신태민
*/
#endregion

public enum EMissionType
{
    Kill,
    Rescue,
    Goal
}

public abstract class MissionBase
{
    public event Action OnClearMission;

    public abstract void StartMission();

    public abstract void CheckClear();

    public virtual void ClearMission()
    {
        OnClearMission?.Invoke();
    }

    public abstract int GetTargetCount();

    public abstract int GetRemainCount();
}

