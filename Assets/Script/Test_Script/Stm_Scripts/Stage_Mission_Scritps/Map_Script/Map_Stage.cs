using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 맵 스크립트
/*
 ▶ 할일
  - 맵별 인덱스, 미션 타입 적스폰위치 , 아이템 스폰 위치 등을 갖고있음
  - 열거형으로 타입을 받고 타입에따른 스폰포스를 내보낼까 고민중..

    - 작업자 신태민
*/
#endregion

public class Map_Stage : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private int _stageNum;
    [SerializeField] private int _subStageNum;
    [SerializeField] private EMissionType _missionType;
    [SerializeField] private Transform _playerSpawnPos;
    [SerializeField] private Transform[] _enemySpawnPos = new Transform[0];
    [SerializeField] private Transform[] _bossSpawnPos = new Transform[0];
    [SerializeField] private Transform[] _boxSpawnPos = new Transform[0];
    [SerializeField] private Transform[] _keySpawnPos = new Transform[0];
    [SerializeField] private Transform[] _citizenSpawnPos = new Transform[0];
    #endregion

    #region 외부 호출 함수
    // 미션 매니저가 플레이어의 레벨(스테이지) 데이터에 따른 반환할 맵을 찾기위함
    public int GetStageNum => _stageNum;
    public int GetSubStageNum => _subStageNum;

    // 스폰위치를 검사해 총 적 수를 반환
    public int GetEnemyCount => _enemySpawnPos.Length + _bossSpawnPos.Length;

    public Transform GetPlayerSpawnPos => _playerSpawnPos;

    // 미션 타입 확인용
    public EMissionType GetMissionType => _missionType;

    // 맵 위치 를 타입별로 반환 하기 위함
    public enum ESpawnPosType
    {
        Enemy,
        Boss,
        Key,
        Box,
        Citizen
    }

    // 들어오는 상태에따른 Spawn 위치 반환
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
