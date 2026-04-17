using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ ProjectileManager

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 오브젝트 풀링 기반의 투사체 관리 및 생성
*/

public class ProjectileManager : MonoBehaviour
{
    public enum EProjectileType
    {
        None,
        Arrow,
        Bullet,
        EnemyRagdoll,
        BossRagdoll
    }

    [System.Serializable]
    public class ProjectileInfo
    {
        public EProjectileType type;
        public GameObject prefab;
        public int prewarmCount;
    }

    #region 인스펙터
    [Header("투사체 정보")]
    [SerializeField] private List<ProjectileInfo> _projectileInfos;
    #endregion

    #region 내부 변수
    public static ProjectileManager Instance { get; private set; }
    private readonly Dictionary<EProjectileType, Queue<GameObject>> _pools = new Dictionary<EProjectileType, Queue<GameObject>>();
    private readonly Dictionary<EProjectileType, ProjectileInfo> _infos = new Dictionary<EProjectileType, ProjectileInfo>();
    private Transform _poolRoot;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < _projectileInfos.Count; i++)
        {
            if (_projectileInfos[i].type == EProjectileType.None ||
                _projectileInfos[i].prefab == null ||
                _projectileInfos[i].prewarmCount <= 0)
            {
                Debug.LogError($"ProjectileManager _projectileInfos[{i}] 인스펙터 확인");
                return;
            }
        }

        InitProjectileInfo();
        InitProjectile();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitProjectileInfo()
    {
        for (int i = 0; i < _projectileInfos.Count; i++)
        {
            ProjectileInfo info = _projectileInfos[i];

            if (info.type != EProjectileType.None && info.prefab != null && info.prewarmCount > 0)
            {
                _infos[info.type] = info;
            }
        }
    }

    private void InitProjectile()
    {
        _poolRoot = new GameObject("Projectile_PoolRoot").transform;

        foreach (var info in _infos.Values)
        {
            if (!_pools.ContainsKey(info.type))
            {
                _pools[info.type] = new Queue<GameObject>();
            }

            for (int j = 0; j < info.prewarmCount; j++)
            {
                GameObject projectile = Instantiate(info.prefab, _poolRoot);
                projectile.SetActive(false);
                _pools[info.type].Enqueue(projectile);
            }
        }
    }

    #region 외부 호출 함수
    public GameObject SpawnProjectile(EProjectileType type)
    {
        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Projectile_PoolRoot").transform;
        }

        if (_pools.TryGetValue(type, out Queue<GameObject> pool) && pool.Count > 0)
        {
            GameObject projectile = _pools[type].Dequeue();
            projectile.transform.SetParent(null);
            projectile.SetActive(true);
            return projectile;
        }

        if (_infos.TryGetValue(type, out ProjectileInfo info))
        {
            GameObject extra = Instantiate(info.prefab);
            extra.transform.SetParent(null);
            extra.SetActive(true);
            return extra;
        }

        return null;
    }

    public void DespawnProjectile(EProjectileType type, GameObject projectile)
    {
        if (projectile == null)
        {
            return;
        }

        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Projectile_PoolRoot").transform;
        }

        projectile.SetActive(false);

        projectile.transform.SetParent(_poolRoot);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (_pools.ContainsKey(type))
        {
            _pools[type].Enqueue(projectile);
        }
    }
    #endregion
}