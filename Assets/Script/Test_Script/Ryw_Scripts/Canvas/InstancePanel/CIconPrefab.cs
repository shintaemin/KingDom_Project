using UnityEngine;


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
    #endregion

    #region 내부 변수
    private Transform _targetTransform;
    private Sprite _iconSprite;
    private EStep _currentStep = EStep.Ready;
    private Vector3 _scale;
    private float _scaleMag;
    #endregion

    void Awake()
    {
        _scale = transform.localScale;
        transform.localScale = Vector3.zero;

        _scaleMag = _scale.magnitude;
    }

    void Start()
    {

    }

    void Update()
    {
        switch (_currentStep)
        {
            // 준비
            case EStep.Ready:
                if (!_iconSprite.IsNull("_iconSprite") &&
                !_targetTransform.IsNull("TargetTransform"))
                {
                    ChageStep(EStep.Spawn);
                    break;
                }
                break;
                // 스폰 애니메이션
            case EStep.Spawn:
                transform.localScale = Vector3.MoveTowards(transform.localScale, _scale, (_scaleMag / SpawnDelay) * Time.deltaTime);
                if (transform.localScale == _scale)
                {
                    ChageStep(EStep.Translate);
                }
                break;
            case EStep.Translate:
                transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, MoveSpeed * Time.deltaTime);
                if (transform.position == _targetTransform.position)
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

    public void SetIcon(Sprite icon)
    {
        _iconSprite = icon;
    }
    public void SetTargetTransform(Transform target)
    {
        _targetTransform = target;
    }
}
