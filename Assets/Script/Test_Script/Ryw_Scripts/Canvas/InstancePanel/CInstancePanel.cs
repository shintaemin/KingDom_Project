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

    public Sprite SkullIcon;
    public Sprite GemIcon;

    [SerializeField] private Transform _skullTargetTransform;
    [SerializeField] private Transform _gemTargetTransform;
    [SerializeField] private Transform _spawnRoot;

    [Header("디버그 키")]
    [SerializeField] private bool _useDebugKey = false;
    [SerializeField] private KeyCode _SpawnKey = KeyCode.S;
    #endregion

    #region 내부 변수

    #endregion

    void Awake()
    {
        if (_iconPrefab.IsNull("_gemPrefab") ||
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
            if (Input.GetKeyDown(_SpawnKey))
            {
                SpawnIcon(EIconType.Skull, Vector3.zero);
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
}
