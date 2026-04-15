using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 스폰 매니저
/*
 ▶ 할일
  - 스폰시킬 모든 프리펩을 들고있는다.
  - 인게임매니저를 통해 전달받은 맵의 적 생성위치 (태그로확인) , 보스생성위치, 키, 보석상자, 시민(구출미션) 확인
  - 생성할 프리펩을 맵에 pos 의 길이만큼 생성

    - 작업자 신태민
*/
#endregion


public class SpawnManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Map_Stage _currentMap;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _bowPrefab;
    [SerializeField] private GameObject _goalBowPrefab;
    [SerializeField] private GameObject _goalZombiePrefab;
    [SerializeField] private GameObject _bossPrefab;
    #endregion

    #region 이벤트
    public event Action<GameObject> OnSpawn;
    #endregion

    private void CheckSpawn()
    {
        if (_currentMap == null)
        {
            return;
        }

        // 가독성을 위해.. 하나씩 생성
        // 각 위치정보를 맵에서 가져온다.
        Transform[] enemy = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Enemy);
        Transform[] zombie = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Zombie);
        Transform[] boss = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Boss);
        Transform[] shield = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Shield);
        Transform[] bow = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Bow);
        Transform[] goalBow = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.GoalBow);
        Transform[] goalZombie = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.GoalZombie);

        // 각 프리펩을 생성할수있도록 태그와 위치를 체크 -- 테스트용 널체크
        if (_enemyPrefab != null && enemy.Length != 0)
        {
            PosCheck(_enemyPrefab, enemy);
        }
        if (_zombiePrefab != null && zombie.Length != 0)
        {
            PosCheck(_zombiePrefab, zombie);
        }
        if (_shieldPrefab != null && shield.Length != 0)
        {
            PosCheck(_shieldPrefab, shield);
        }
        if (_bowPrefab != null && bow.Length != 0)
        {
            PosCheck(_bowPrefab, bow);
        }
        if (_goalBowPrefab != null && goalBow.Length != 0)
        {
            PosCheck(_goalBowPrefab, goalBow);
        }
        if (_goalZombiePrefab != null && goalZombie.Length != 0)
        {
            PosCheck(_goalZombiePrefab, goalZombie);
        }
        if (_bossPrefab != null && boss.Length != 0)
        {
            PosCheck(_bossPrefab, boss);
        }

        Transform player = _currentMap.GetPlayerSpawnPos;

        if (_playerPrefab != null)
        {
            Spawn(_playerPrefab, player);
        }
    }

    // Transform 체크 함수
    private void PosCheck(GameObject prefab, Transform[] pos)
    {
        if (prefab == null || pos == null || pos.Length == 0)
        {
            return;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (pos[i] == null || prefab == null)
            {
                continue;
            }

            Transform tr = pos[i];
            Spawn(prefab, tr);
        }
    }

    // 프리펩과 Transform 전달받아 생성
    private void Spawn(GameObject prefab, Transform pos)
    {
        GameObject go = Instantiate(prefab);
        Transform tr = pos;
        go.transform.position = tr.position;
        go.transform.rotation = Quaternion.identity;
        // 필요하다면 스케일까지

        OnSpawn?.Invoke(go);
        Debug.Log($"[SpawnManager] : {go.name} 생성 완료");
    }

    #region 외부 호출 함수
    public void SetMap(Map_Stage map)
    {
        if (_currentMap != null)
        {
            MapClear();
        }

        GameObject go = Instantiate(map.gameObject, Vector3.zero, Quaternion.identity);

        _currentMap = go.GetComponent<Map_Stage>();
    }

    public void MapClear()
    {
        if (_currentMap == null)
        {
            return;
        }

        Destroy(_currentMap.gameObject);
        _currentMap = null;
    }

    public Map_Stage GetCurrentMap => _currentMap;

    public void SpawnStart()
    {
        if (_currentMap == null)
        {
            return;
        }

        CheckSpawn();
    }
    #endregion
}