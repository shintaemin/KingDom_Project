using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인스펙터
/*
 ▶ 할일
  - 모든 맵을 관리할 Registry
  - 인덱스값으로 꺼내쓸수 잇음 (플레이어 레벨(스테이지))
*/
#endregion

[CreateAssetMenu(menuName ="Create_SO/MapData", fileName = "Map_Registry")]
public class Map_Registry_SO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private List<Map_Stage> _maps = new List<Map_Stage>(); 
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

            Map_Stage map = _maps[i];
            int mapStage = map.GetStageNum;
            int subStage = map.GetSubStageNum;

            if (mapStage != stageNum)
            {
                continue;
            }

            if (subStage != subStageNum)
            {
                continue;
            }

            return map;
        }

        return null;
    }
#endregion

    
}
