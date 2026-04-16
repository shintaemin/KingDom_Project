using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EffectManager

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class EffectManager : MonoBehaviour
{
    public enum EEffectType
    {
        None,
        RangedAttack,
        ZombieSmoke,
        QuestionMark,
        SurprisedMark,
        BossJump,
        BossJumpEnd,
        BossRoar,
        ClickEnemy,
        DeadBlood,
        DeadCircle,
        PlayerDamaged,
        PlayerHit,
        Block,
        Block2,


    }

    [System.Serializable]
    public class EffectInfo
    {
        public EEffectType type;
        public ESfxType sfxType;
        public GameObject prefab;
        public int prewarmCount = 10;
    }

    #region 인스펙터
    [Header("이펙트 정보")]
    [SerializeField] private List<EffectInfo> _effectInfos;
    #endregion

    #region 내부 변수
    public static EffectManager Instance { get; private set; }
    private readonly Dictionary<EEffectType, Queue<GameObject>> _pools = new Dictionary<EEffectType, Queue<GameObject>>();
    private readonly Dictionary<EEffectType, EffectInfo> _infos = new Dictionary<EEffectType, EffectInfo>();
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

        for (int i = 0; i < _effectInfos.Count; i++)
        {
            if (_effectInfos[i].type == EEffectType.None ||
                _effectInfos[i].prefab == null ||
                _effectInfos[i].prewarmCount <= 0)
            {
                Debug.LogError($"EffectManager _effectInfos[{i}] 인스펙터 확인");
                return;
            }
        }

        InitEffectInfo();
        InitEffect();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitEffectInfo()
    {
        for (int i = 0; i < _effectInfos.Count; i++)
        {
            EffectInfo info = _effectInfos[i];

            if (info.type != EEffectType.None && info.prefab != null && info.prewarmCount > 0)
            {
                _infos[info.type] = info;
            }
        }
    }

    private void InitEffect()
    {
        _poolRoot = new GameObject("Effect_PoolRoot").transform;

        foreach (var info in _infos.Values)
        {
            if (!_pools.ContainsKey(info.type))
            {
                _pools[info.type] = new Queue<GameObject>();
            }

            for (int i = 0; i < info.prewarmCount; i++)
            {
                GameObject effect = Instantiate(info.prefab, _poolRoot);
                effect.SetActive(false);
                _pools[info.type].Enqueue(effect);
            }
        }
    }

    #region 외부 호출 함수
    public GameObject SpawnEffect(EEffectType type, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Effect_PoolRoot").transform;
        }

        if (_pools.TryGetValue(type, out Queue<GameObject> pool) && pool.Count > 0)
        {
            GameObject effect = _pools[type].Dequeue();
            effect.transform.SetPositionAndRotation(position, rotation);
            effect.transform.SetParent(parent != null ? parent : _poolRoot);
            effect.SetActive(true);

            if (_infos.TryGetValue(type, out EffectInfo ei))
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.SFXPlay(ei.sfxType, true);
                }

                //else
                //{
                //    Debug.LogWarning("사운드 매니저 인스턴스 = Null");
                //}
            }

            return effect;
        }

        if (_infos.TryGetValue(type, out EffectInfo info))
        {
            GameObject extra = Instantiate(info.prefab);
            extra.transform.SetPositionAndRotation(position, rotation);
            extra.transform.SetParent(parent != null ? parent : _poolRoot);
            extra.SetActive(true);

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SFXPlay(info.sfxType, true);
            }

            //else
            //{
            //    Debug.LogWarning("사운드 매니저 인스턴스 = Null");
            //}

            return extra;
        }

        return null;
    }

    public void DespawnEffect(EEffectType type, GameObject effect)
    {
        if (effect == null)
        {
            return;
        }

        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Effect_PoolRoot").transform;
        }

        effect.SetActive(false);

        effect.transform.SetParent(_poolRoot);

        if (_pools.ContainsKey(type))
        {
            _pools[type].Enqueue(effect);
        }
    }
    #endregion
}