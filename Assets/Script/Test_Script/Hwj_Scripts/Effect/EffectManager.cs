using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public enum EEffectType
    {
        None,
        Player_Attack,
        Enemy_Attack,
        
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

        InitEffect();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitEffect()
    {
        _poolRoot = new GameObject("Effect_PoolRoot").transform;

        for (int i = 0; i < _effectInfos.Count; i++)
        {
            EEffectType type = _effectInfos[i].type;

            if (!_pools.ContainsKey(type))
            {
                _pools[type] = new Queue<GameObject>();
            }

            for (int j = 0; j < _effectInfos[i].prewarmCount; j++)
            {
                GameObject effect = Instantiate(_effectInfos[i].prefab, _poolRoot);
                effect.SetActive(false);
                _pools[type].Enqueue(effect);
            }
        }
    }

    #region 외부 호출 함수
    public GameObject SpawnEffect(EEffectType type, Vector3 position, Quaternion rotation)
    {
        if (_poolRoot == null)
        {
            _poolRoot = new GameObject("Effect_PoolRoot").transform;
        }

        EffectInfo info = _effectInfos.Find(effect => effect.type == type);

        if (info == null)
        {
            return null;
        }

        GameObject effect = null;

        if (_pools.ContainsKey(type) && _pools[type].Count > 0)
        {
            effect = _pools[type].Dequeue();
            effect.transform.SetParent(null);
        }

        else
        {
            effect = Instantiate(info.prefab);
        }

        if (effect == null)
        {
            return null;
        }

        effect.transform.SetPositionAndRotation(position, rotation);

        effect.SetActive(true);

        SoundManager.Instance.SFXPlay(info.sfxType);

        return effect;
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