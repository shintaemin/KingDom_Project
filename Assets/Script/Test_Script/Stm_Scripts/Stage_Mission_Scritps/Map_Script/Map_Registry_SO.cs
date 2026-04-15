using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인스펙터
/*
 ▶ 할일
  - 모든 맵을 관리할 Registry
  - 인덱스값으로 꺼내쓸수 잇음 (플레이어 레벨(스테이지))

    - 작업자 신태민
*/
#endregion

[System.Serializable]
public class Map_List
{
    [SerializeField] private List<Map_Stage> _maps = new List<Map_Stage>();
    [SerializeField] private int _stageNum;

    public int GetStageNum => _stageNum;
    public int GetStageCount => _maps.Count;

    public Map_Stage FindMap(int stageNum, int subStageNum)
    {
        if (_maps.Count == 0)
        {
            return null;
        }

        for (int i = 0; i < _maps.Count; i++)
        {
            if (_maps[i] == null)
            {
                continue;
            }

            Map_Stage map = _maps[i];
            int mapStage = map.GetStageNum;
            int subStage = map.GetSubStageNum;

            if (mapStage != stageNum || subStage != subStageNum)
            {
                continue;
            }

            return map;
        }

        return null;
    }
}

[CreateAssetMenu(menuName ="Create_SO/MapData", fileName = "Map_Registry")]
public class Map_Registry_SO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private List<Map_List> _maps = new List<Map_List>(); 
    #endregion

    #region 외부 호출 함수
    public Map_Stage GetMap(int stageNum, int subStageNum)
    {
        if (_maps.Count == 0)
        {
            return null;
        }

        for (int i = 0; i < _maps.Count; i++)
        {
            if (_maps[i] == null)
            {
                continue;
            }

            Map_List maps = _maps[i];
            int stage = maps.GetStageNum;

            if (stage != stageNum)
            {
                continue;
            }

            Map_Stage map = maps.FindMap(stageNum, subStageNum);

            if (map != null)
            {
                return map;
            }
        }

        return null;
    }

    public int GetStageCount(int stageNum)
    {
        if (_maps.Count == 0)
        {
            return -1;
        }

        for (int i = 0; i < _maps.Count; i++)
        {
            if (_maps[i] == null)
            {
                continue;
            }

            Map_List map = _maps[i];
            int stage = map.GetStageNum;

            if (stage != stageNum)
            {
                continue;
            }

            int current = map.GetStageCount;
            return current;
        }

        return -1;
    }
    #endregion
}
