using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 미션 베이스
/*
 ▶ 할일
  - 상속 기반으로 진행
*/
#endregion

public enum EMissionType
{
    None,
    Kill,
    Rescue,
    Goal
}

public abstract class MissionBase : MonoBehaviour
{
    public event Action OnClearMission;

    public abstract void StartMission();

    public abstract void CheckClear();

    public virtual void ClearMission()
    {
        OnClearMission?.Invoke();
    }
}
