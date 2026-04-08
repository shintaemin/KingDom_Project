using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ ProjectileFactory

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 오브젝트 풀링 기반의 투사체 관리 및 생성
*/

public class ProjectileFactory : MonoBehaviour
{
    public enum ProjectileType
    {
        None,
        Arrow,
        Bullet
    }

    [System.Serializable]
    public class ProjectileInfo
    {
        public ProjectileType type;
        public GameObject prefab;
        public int prewarmCount;
    }

    #region 인스펙터
    [Header("투사체 정보")]
    [SerializeField] private List<ProjectileInfo> _projectileInfos;
    #endregion

    #region 내부 변수
    public static ProjectileFactory Instance { get; private set; }
    private readonly Dictionary<ProjectileType, Queue<GameObject>> _pools = new Dictionary<ProjectileType, Queue<GameObject>>();
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
            if (_projectileInfos[i].type == ProjectileType.None ||
                _projectileInfos[i].prefab == null ||
                _projectileInfos[i].prewarmCount <= 0)
            {
                Debug.LogError($"ProjectileFactory _projectileInfos[{i}] 인스펙터 확인");
                return;
            }
        }

        InitProjectile();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitProjectile()
    {
        _poolRoot = new GameObject("Projectile_PoolRoot").transform;

        for (int i = 0; i < _projectileInfos.Count; i++)
        {
            ProjectileType type = _projectileInfos[i].type;

            if (!_pools.ContainsKey(type))
            {
                _pools[type] = new Queue<GameObject>();
            }

            for (int j = 0; j < _projectileInfos[i].prewarmCount; j++)
            {
                GameObject projectile = Instantiate(_projectileInfos[i].prefab, _poolRoot);
                projectile.SetActive(false);
                _pools[type].Enqueue(projectile);
            }
        }
    }

    #region 외부 호출 함수
    public GameObject SpawnProjectile(ProjectileType type)
    {
        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Projectile_PoolRoot").transform;
        }

        if (_pools.ContainsKey(type) && _pools[type].Count > 0)
        {
            GameObject projectile = _pools[type].Dequeue();
            projectile.transform.SetParent(null);
            projectile.SetActive(true);
            return projectile;
        }

        // 리스트에서 본인 타입과 같은 타입을 가진 ProjectileInfo를 찾는다.
        ProjectileInfo info = _projectileInfos.Find(projectile => projectile.type == type);

        if (info != null)
        {
            GameObject extra = Instantiate(info.prefab);
            extra.transform.SetParent(null);
            extra.SetActive(true);
            return extra;
        }

        return null;
    }

    public void DespawnProjectile(ProjectileType type, GameObject projectile)
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