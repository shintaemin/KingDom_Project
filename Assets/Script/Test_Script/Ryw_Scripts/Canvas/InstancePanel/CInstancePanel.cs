using UnityEngine;


#region CInstancePanel
/*
▶ 작성자 류연우
*/
#endregion

public class CInstancePanel : MonoBehaviour
{
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
                SpawnIcon(SkullIcon, Vector3.zero);
            }
        }
    }

    public void SpawnIcon(Sprite icon, Vector3 position)
    {
        // 생성
        // 아이콘 설정
        // 타겟 설정.
        GameObject go = Instantiate(_iconPrefab, _spawnRoot);
        if (go.TryGetComponent(out CIconPrefab prefab))
        {
            prefab.SetIcon(icon);
            prefab.SetTargetTransform(_skullTargetTransform);
        }
    }
}
