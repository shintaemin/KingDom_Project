using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 킬 미션
/*
 ▶ 할일
  - 킬 미션을 정의
  - 단순하게 맵에 있는
*/
#endregion


public class Kill_Mission : MissionBase
{
    #region 인스펙터
    [SerializeField] private EMissionType _type = EMissionType.Kill;
    [SerializeField] private string _enemySpawnPosTag;
    [SerializeField] private int _targetKillCount;
    [SerializeField] private int _remainKillCount;
    #endregion

    private void ResetData()
    {
        _targetKillCount = 0;
        _remainKillCount = 0;
    }

    #region 외부 호출 함수
    public override void StartMission()
    {
        ResetData();

        Transform[] children = GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] == null)
            {
                continue;
            }

            Transform pos = children[i];

            if (pos.CompareTag(_enemySpawnPosTag))
            {
                // 스포너 자리 딱인데
                // 캐싱자리
                _targetKillCount++;
            }
        }

        Debug.Log($"[Kill_Mission] : 타겟갯수 : {_targetKillCount}");
    }

    public override void CheckClear()
    {
        _remainKillCount++;

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
