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
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private GameObject _eBossPrefab;
    [SerializeField] private GameObject _zBossPrefab;
    [SerializeField] private GameObject _keyPrefab;
    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private GameObject _citizenPrefab;
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
        Transform[] boss = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Boss);
        Transform[] key = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Key);
        Transform[] box = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Box);
        Transform[] citizen = _currentMap.GetSpawnPos(Map_Stage.ESpawnPosType.Citizen);

        // 각 프리펩을 생성할수있도록 태그와 위치를 체크 -- 테스트용 널체크
        if (_enemyPrefab != null && _zombiePrefab != null)
        {
            PosCheck(_enemyPrefab, _zombiePrefab, enemy, "EnemySpawnPos", "ZombieSpawnPos");
        }
        if (_eBossPrefab != null && _zBossPrefab != null)
        {
            PosCheck(_eBossPrefab, _zBossPrefab, boss, "eBossSpawnPos", "zBossSpawnPos");
        }
        if (_keyPrefab != null)
        {
            PosCheck(_keyPrefab, key);
        }
        if (_keyPrefab != null)
        {
            PosCheck(_boxPrefab, box);
        }
        if (_citizenPrefab != null)
        {
            PosCheck(_citizenPrefab, citizen);
        }
    }

    // Transform 체크 함수
    private void PosCheck(GameObject prefab, Transform[] pos)
    {
        if (prefab == null)
        {
            return;
        }

        if (pos == null || pos.Length == 0)
        {
            return;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (pos[i] == null)
            {
                continue;
            }

            Transform tr = pos[i];

            if (prefab != null)
            {
                Spawn(prefab, tr);
            }
        }
    }

    private void PosCheck(GameObject prefab_1, GameObject prefab_2, Transform[] pos, string tag_1, string tag_2)
    {
        if (prefab_1 == null ||  prefab_2 == null)
        {
            return;
        }
        if (pos == null || pos.Length == 0)
        {
            return;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (pos[i] == null)
            {
                continue;
            }

            Transform tr = pos[i];

            // 지정한 번호 태그가 맞다면 해당 프리펩 전달
            if (tr.CompareTag(tag_1))
            {
                if (prefab_1 != null)
                {
                    Spawn(prefab_1, tr);
                }
                continue;
            }
            if (tr.CompareTag(tag_2))
            {
                if (prefab_2 != null)
                {
                    Spawn(prefab_2, tr);
                }
                continue;
            }
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
            return;
        }

        GameObject go = Instantiate(map.gameObject, Vector3.zero, Quaternion.identity);

        _currentMap = go.GetComponent<Map_Stage>();
        CheckSpawn();
    }
    #endregion
}