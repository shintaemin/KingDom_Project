using UnityEngine;
using UnityEngine.UI;


#region CIconPrefab
/*
▶ 작성자 류연우

사용 시 SetIcon, SetTargetTransform 을 호출하면 알아서 다 한다.
*/
#endregion

public class CIconPrefab : MonoBehaviour
{
    public enum EStep
    {
        Ready,
        Spawn,
        Translate,
        Destroy
    }
    #region 인스펙터
    public float MoveSpeed = 5f;
    public float SpawnDelay = 0.5f;
    public float DestroyDelay = 1f;
    [SerializeField] private Image _image;
    [SerializeField] private float _explosionSpeed = 5f;
    [SerializeField] private float _epsilon = float.Epsilon;
    #endregion

    #region 내부 변수
    private Transform _targetTransform;
    private Sprite _iconSprite;
    private EStep _currentStep = EStep.Ready;
    private Vector3 _scale;
    private float _scaleMag;

    private bool _isReady = false;
    private bool _explosionEffect = false;
    private bool _isWolrdPos;
    private Vector3 _explosionDir;
    #endregion

    void Awake()
    {
        if (_image.IsNull("_image"))
        {
            return;
        }
        _scale = transform.localScale;

        _scaleMag = _scale.magnitude;
    }

    void Start()
    {
        transform.localScale = Vector3.zero;

    }

    void Update()
    {
        switch (_currentStep)
        {
            // 준비
            case EStep.Ready:
                if (_isReady)
                    ChageStep(EStep.Spawn);
                break;
            // 스폰 애니메이션
            case EStep.Spawn:
                transform.localScale = Vector3.MoveTowards(transform.localScale, _scale, (_scaleMag / SpawnDelay) * Time.deltaTime);
                if (_explosionEffect)
                {
                    // 폭발하는 효과를 준다.
                    // 랜덤한 방향으로
                    // 랜덤한 거리를 이동한다.
                    transform.Translate(_explosionDir * _explosionSpeed * Time.deltaTime);
                }
                if (Vector3.Distance(transform.localScale, _scale) < _epsilon)
                {
                    ChageStep(EStep.Translate);
                }
                break;
            case EStep.Translate:
                if (_isWolrdPos)
                {
                    CInGameCanvas.WorldToUI(_targetTransform.position, out Vector3 uiPos);
                    transform.position = Vector3.MoveTowards(transform.position, uiPos, MoveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, MoveSpeed * Time.deltaTime);
                }
                if (Vector3.Distance(transform.position, _targetTransform.position) < _epsilon)
                {
                    ChageStep(EStep.Destroy);
                }
                break;
            // 삭제 애니메이션
            case EStep.Destroy:
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, _scaleMag / DestroyDelay * Time.deltaTime);
                break;
        }
    }

    public void ChageStep(EStep step)
    {
        if (_currentStep == step) return;

        //exit
        switch (_currentStep)
        {
            case EStep.Spawn: break;
            case EStep.Translate: break;
            case EStep.Destroy: break;
        }

        _currentStep = step;

        //enter
        switch (_currentStep)
        {
            case EStep.Spawn:
                break;
            case EStep.Translate:
                break;
            case EStep.Destroy:
                Destroy(gameObject, DestroyDelay);
                break;
        }
    }



    public void Init(Sprite icon, Transform target, bool explosionEffect = false, bool isWolrdPos = false)
    {
        SetIcon(icon);
        SetTargetTransform(target);
        _explosionEffect = explosionEffect;
        if (_explosionEffect)
        {
            _explosionDir = Random.onUnitSphere;
        }
        _isReady = true;
    }
    private void SetIcon(Sprite icon)
    {
        //Debug.Log("Set Icon");
        _iconSprite = icon;
        _image.sprite = icon;

    }
    private void SetTargetTransform(Transform target, bool isWolrdPos = false)
    {
        _isWolrdPos = isWolrdPos;
        //Debug.Log("Set Transform");
        _targetTransform = target;
        _targetTransform.IsNull("_targetTransform");
    }
}
