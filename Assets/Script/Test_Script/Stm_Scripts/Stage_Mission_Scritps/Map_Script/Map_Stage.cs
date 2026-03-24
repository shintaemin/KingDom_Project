using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 맵 스크립트
/*
 ▶ 할일
  - 맵별 인덱스, 미션 타입 적스폰위치 , 아이템 스폰 위치 등을 갖고있음
  - 열거형으로 타입을 받고 타입에따른 스폰포스를 내보낼까 고민중..
*/
#endregion

public class Map_Stage : MonoBehaviour
{
    public enum ESpawnPosType
    {
        Enemy,
        Boss,
        Key,
        Box,
        Citizen
    }

    #region 인스펙터
    [SerializeField] private int _stageNum;
    [SerializeField] private int _subStageNum;
    [SerializeField] private EMissionType _missionType;
    [SerializeField] private Transform[] _enemySpawnPos;
    [SerializeField] private Transform[] _bossSpawnPos;
    [SerializeField] private Transform[] _boxSpawnPos;
    [SerializeField] private Transform[] _keySpawnPos;
    [SerializeField] private Transform[] _citizenSpawnPos;
    #endregion

    #region 외부 호출 함수
    // 미션 매니저가 플레이어의 레벨(스테이지) 데이터에 따른 내보낼 맵을 찾기위함
    public int GetStageNum => _stageNum;
    public int GetSubStageNum => _subStageNum;
    public int GetEnemyPos => _bossSpawnPos != null ? _enemySpawnPos.Length + 1 : _enemySpawnPos.Length;
    public EMissionType GetMissionType => _missionType;
    public Transform[] GetSpawnPos(ESpawnPosType type)
    {
        switch (type)
        {
            case ESpawnPosType.Enemy: return _enemySpawnPos;
            case ESpawnPosType.Boss: return _bossSpawnPos;
            case ESpawnPosType.Key: return _keySpawnPos;
            case ESpawnPosType.Box: return _boxSpawnPos;
            case ESpawnPosType.Citizen: return _citizenSpawnPos;
        }

        return null;
    }
    #endregion
}
