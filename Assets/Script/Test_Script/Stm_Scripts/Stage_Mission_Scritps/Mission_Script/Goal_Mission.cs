using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion


public class Goal_Mission : MissionBase
{
    [SerializeField] private int _targetCount;
    [SerializeField] private int _currentCount;

    public Goal_Mission(int target)
    {
        _targetCount = target;
        _currentCount = 0;
    }
    private void ResetData()
    {
        // ±¸µ¶ ÇØÁ¦
        _targetCount = 0;
        _currentCount = 0;
    }

    public override void StartMission()
    {
        Debug.Log($"[Goal_Mission] : Å¸°Ù°¹¼ö : {_targetCount}");
    }

    public override void CheckClear()
    {
        _currentCount++;
        if (_currentCount >= _targetCount)
        {
            ClearMission();
        }
    }

    public override void ClearMission()
    {
        base.ClearMission();
        ResetData();
    }

    public override int GetTargetCount() => _targetCount;
    public override int GetRemainCount() => _currentCount;
}
