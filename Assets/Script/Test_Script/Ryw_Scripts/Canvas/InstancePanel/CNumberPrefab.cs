using System.Collections;
using UnityEngine;
using UnityEngine.UI;


#region CNumberPrefab
/*
▶ 작성자 류연우
*/
#endregion

public class CNumberPrefab : MonoBehaviour
{
    public enum EStep
    {
        Ready,
        Spawn,
        Translate,
        Destroy
    }
    #region 인스펙터
    public float SpawnDelay = 0.5f;

    public Vector2 MoveOffeset = new Vector2(0,10);
    public float MoveDelay = 1f;

    public float DestroyDelay = 1f;
    public Sprite[] Sprites;
    [SerializeField] private CanvasGroup _canvasGroup;
    #endregion

    #region 내부 변수
    private Image[] _numberImage;

    private Color _color;
    private EStep _currentStep = EStep.Ready;
    private bool _isReady = false;

    private Coroutine _co;

    private Vector3 _targetPosition;
    private float moveSpeed;
    #endregion

    void Awake()
    {
        Sprites.IsNull("Sprites");
        if(_canvasGroup.IsNull("_canvasGroup"))
        {
            return;
        }
        _canvasGroup.alpha = 0;
        _targetPosition = transform.localPosition + (Vector3)MoveOffeset;
        moveSpeed = MoveOffeset.magnitude / MoveDelay;
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
                if (!_isReady)
                {
                    return;
                }
                ChageStep(EStep.Spawn);
                break;
            // 스폰 애니메이션
            case EStep.Spawn:
                break;
            case EStep.Translate:
                // 점점 올라간다. 이것도 코루틴?
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
                if (transform.position == _targetPosition)
                {
                    ChageStep(EStep.Destroy);
                }
                break;
            // 삭제 애니메이션
            case EStep.Destroy:
                break;
        }
    }

    private IEnumerator SetAlpha(bool flag, bool nextStep = true)
    {
        while (true)
        {
            if (flag)
            {
                _canvasGroup.alpha += Time.deltaTime / SpawnDelay;
                if (_canvasGroup.alpha >= 1)
                {
                    _canvasGroup.alpha = 1;
                    break;
                }
            }
            else
            {
                _canvasGroup.alpha -= Time.deltaTime / DestroyDelay;
                if (_canvasGroup.alpha <= 0)
                {
                    _canvasGroup.alpha = 0;
                    break;
                }
            }
            yield return null;
        }

        if (nextStep)
            NextStep();
    }
    public void ChageStep(EStep step)
    {
        if (_currentStep == step) return;

        //exit
        switch (_currentStep)
        {
            case EStep.Spawn:
                if (_co != null)
                {
                    StopCoroutine(_co);
                    _co = null;
                    _canvasGroup.alpha = 1;
                }
                break;
            case EStep.Translate: break;
            case EStep.Destroy:
                if (_co != null)
                {
                    StopCoroutine(_co);
                    _co = null;
                    _canvasGroup.alpha = 0;
                }
                break;
        }

        _currentStep = step;

        //enter
        switch (_currentStep)
        {
            case EStep.Spawn:
                _co = StartCoroutine(SetAlpha(true));
                break;
            case EStep.Translate:
                break;
            case EStep.Destroy:
                _co = StartCoroutine(SetAlpha(false, false));
                Destroy(gameObject, DestroyDelay);
                break;
        }
    }

    private void NextStep()
    {
        int i = (int)_currentStep;
        ChageStep((EStep)(i + 1));
    }

    public void InitData(string number, Color color)
    {
        _numberImage = new Image[number.Length];

        float length = 0;

        // string에 맞게 이미지 생성 후 캐싱
        for (int i = 0; i < number.Length; i++)
        {
            GameObject go = new GameObject($"NumberObejct{i}", typeof(Image));
            // 부모 조정.
            go.transform.SetParent(transform, false);

            if (go.TryGetComponent(out Image image))
            {
                image.sprite = Sprites[number[i] - '0'];
                _numberImage[i] = image;
                image.SetNativeSize();
            }
            // 위치, 크기 조정.
            go.transform.position = new Vector3(length, 0, 0);
            // length 만큼 밀어준다.
            // length 업데이트
            length += image.sprite.rect.width;
        }

        _color = color;
        // 색상 배정.
        // 캐싱된 이미지들 컬러 수정.
        for (int i = 0; i < number.Length; i++)
        {
            _numberImage[i].color = _color;
        }

        _isReady = true;
    }
}
