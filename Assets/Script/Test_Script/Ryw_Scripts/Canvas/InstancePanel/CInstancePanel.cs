using UnityEngine;


#region CInstancePanel
/*
▶ 작성자 류연우
*/
#endregion

public class CInstancePanel : MonoBehaviour
{
    public enum EIconType
    {
        Skull,
        Gem
    }
    #region 인스펙터
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private GameObject _numberPrefab;

    public Sprite SkullIcon;
    public Sprite GemIcon;

    [SerializeField] private Transform _skullTargetTransform;
    [SerializeField] private Transform _gemTargetTransform;
    [SerializeField] private Transform _spawnRoot;

    [Header("디버그 키")]
    [SerializeField] private bool _useDebugKey = false;
    [SerializeField] private KeyCode _SpawnIconKey = KeyCode.I;
    [SerializeField] private KeyCode _SpawnNumber = KeyCode.N;
    private bool _flipFlag = false;

    [SerializeField] private Transform _targetTransform;
    #endregion

    #region 내부 변수

    #endregion

    void Awake()
    {
        if (_iconPrefab.IsNull("_gemPrefab") ||
            _numberPrefab.IsNull("_numberPrefab") ||
            _skullTargetTransform.IsNull("_skullTargetTransform") ||
            _gemTargetTransform.IsNull("_gemTargetTransform") ||
            _spawnRoot.IsNull("_spawnRoot")
            )
        {
            return;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (_useDebugKey)
        {
            

            if (Input.GetKeyDown(_SpawnIconKey))
            {
                Vector3 position = Vector3.zero;
                if (_targetTransform != null)
                {
                    if (CInGameCanvas.WorldToUI(_targetTransform.position, out Vector3 uIPos))
                    {
                        position = uIPos;
                    }
                }

                _flipFlag = !_flipFlag;
                if (_flipFlag)
                    SpawnIcon(EIconType.Skull, position);
                else
                    SpawnIcon(EIconType.Gem, position);
            }
            if (Input.GetKeyDown(_SpawnNumber))
            {
                Vector3 position = Vector3.zero;
                if (_targetTransform != null)
                {
                    if (CInGameCanvas.WorldToUI(_targetTransform.position, out Vector3 uIPos))
                    {
                        position = uIPos;
                    }
                }
                SpawnNumber("0123456789", Color.red, position);
            }
        }
    }

    public void SpawnIcon(EIconType type, Vector3 position)
    {
        GameObject go = Instantiate(_iconPrefab, _spawnRoot);
        if (go.IsNull("gameObject"))
        {
            return;
        }
        go.transform.position = position;
        if (go.TryGetComponent(out CIconPrefab prefab))
        {
            switch (type)
            {
                case EIconType.Skull:
                    prefab.SetIcon(SkullIcon);
                    prefab.SetTargetTransform(_skullTargetTransform);
                    break;
                case EIconType.Gem:
                    prefab.SetIcon(GemIcon);
                    prefab.SetTargetTransform(_gemTargetTransform);
                    break;
            }
        }

    }

    // 색도 type으로 결정.
    public void SpawnNumber(string number, Color color, Vector3 position)
    {
        GameObject go = Instantiate(_numberPrefab, _spawnRoot);
        if (go.IsNull("gameObject"))
        {
            return;
        }
        go.transform.position = position;
        if (go.TryGetComponent(out CNumberPrefab prefab))
        {
            prefab.InitData(number, color);
        }
    }
}
