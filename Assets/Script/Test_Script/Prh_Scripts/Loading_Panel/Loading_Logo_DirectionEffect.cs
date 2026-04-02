using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#region 로딩 로고 방향 연출
/*
 ▶ 할일
  - 메인 로고와 잔상 로고의 위치 / 회전 / 알파값을 제어
  - 1차, 2차 연출을 순서대로 재생
  - 활성화 / 비활성화 시 로고 상태를 초기화
  - 핑크 / 하늘 로고는 잔상 연출용으로 사용
  - 각 단계의 시간과 이동값은 인스펙터에서 조정 가능

  - 박라희
 */
#endregion

public class Loading_Logo_DirectionEffect : MonoBehaviour
{
    #region 인스펙터
    [Header("참조")]
    [SerializeField] private RectTransform _mainLogo;
    [SerializeField] private RectTransform _redLogo;
    [SerializeField] private RectTransform _blueLogo;
    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _redImage;
    [SerializeField] private Image _blueImage;

    [Header("1차 연출")]
    // 위치 이동
    [SerializeField] private Vector2 _firstRedOffset = new Vector2(-18f, 6f);
    [SerializeField] private Vector2 _firstBlueOffset = new Vector2(18f, -6f);
    // 회전 각도
    [SerializeField] private float _firstRotateZ = -6f;
    // 퍼지는 시간
    [SerializeField] private float _firstSpreadDuration = 0.18f;
    // 유지 시간
    [SerializeField] private float _firstHoldDuration = 0.07f;
    // 돌아오는 시간
    [SerializeField] private float _firstReturnDuration = 0.18f;

    [Header("2차 연출")]
    [SerializeField] private Vector2 _secondRedOffset = new Vector2(-10f, 0f);
    [SerializeField] private Vector2 _secondBlueOffset = new Vector2(10f, 0f);
    [SerializeField] private float _secondSpreadDuration = 0.06f;
    [SerializeField] private float _secondHoldDuration = 0.03f;
    [SerializeField] private float _secondReturnDuration = 0.08f;

    [Header("공통")]
    // 시작전 대기 시간
    [SerializeField] private float _startDelay = 0.25f;
    // 1차, 2차 사이 대기 시간
    [SerializeField] private float _betweenDelay = 0.08f;
    // 투명도 설정 (잔상용) (0 ~ 1)
    [SerializeField] private float _ghostAlpha = 0.65f;
    #endregion

    #region 내부 변수
    // 시작 위치
    private Vector2 _mainOriginPosition;
    private Vector2 _redOriginPosition;
    private Vector2 _blueOriginPosition;

    // 시작 회전값
    private Quaternion _mainOriginRotation;
    private Quaternion _redOriginRotation;  
    private Quaternion _blueOriginRotation;

    // 현재 실행중인 코루틴 저장
    private Coroutine _playRoutine;
    #endregion

    private void Awake()
    {
        // 시작 위치/회전 저장
        CacheOriginState();
        // 초기 상태
        ResetState();
    }

    private void OnEnable()
    {
        // 기존 코루틴 있으면 종료
        StopPlayRoutine();
        // 연출 시작
        _playRoutine = StartCoroutine(CoPlayRoutine());
    }

    private void OnDisable()
    {
        // 꺼질 때 코루틴 종료
        StopPlayRoutine();
        // 상태 초기화
        ResetState();
    }

    // 초기화
    // 로고의 시작 위치 / 회전값 저장,캐싱
    private void CacheOriginState()
    {
        if (_mainLogo != null)
        {
            // 위치 저장
            _mainOriginPosition = _mainLogo.anchoredPosition;
            // 회전 저장
            _mainOriginRotation = _mainLogo.localRotation;
        }

        if (_redLogo != null)
        {
            _redOriginPosition = _redLogo.anchoredPosition;
            _redOriginRotation = _redLogo.localRotation;
        }

        if (_blueLogo != null)
        {
            _blueOriginPosition = _blueLogo.anchoredPosition;
            _blueOriginRotation = _blueLogo.localRotation;
        }
    }

    // 재생 중인 코루틴 정지
    private void StopPlayRoutine()
    {
        if (_playRoutine == null)
        {
            return;
        }

        // 실행 중지
        StopCoroutine(_playRoutine);
        _playRoutine = null;
    }

    // 로고 초기상태로 되돌리기
    private void ResetState()
    {
        // 위치 + 회전 원래대로 복구
        if (_mainLogo != null)
        {
            _mainLogo.anchoredPosition = _mainOriginPosition;
            _mainLogo.localRotation = _mainOriginRotation;
        }

        if (_redLogo != null)
        {
            _redLogo.anchoredPosition = _redOriginPosition;
            _redLogo.localRotation = _redOriginRotation;
        }

        if (_blueLogo != null)
        {
            _blueLogo.anchoredPosition = _blueOriginPosition;
            _blueLogo.localRotation = _blueOriginRotation;
        }

        // 알파 초기화
        // 메인 보이기
        SetImageAlpha(_mainImage, 1f);
        // 잔상 숨김
        SetImageAlpha(_redImage, 0f);
        SetImageAlpha(_blueImage, 0f);
    }

    // 2차 연출 시작 전 정렬된 상태 
    private void ForceStraightState()
    {
        if (_mainLogo != null)
        {
            // 회전 0
            _mainLogo.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (_redLogo != null)
        {
            _redLogo.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (_blueLogo != null)
        {
            _blueLogo.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // 알파 설정
        SetImageAlpha(_mainImage, 1f);
        SetImageAlpha(_redImage, 0f);
        SetImageAlpha(_blueImage, 0f);
    }

    
    // 전체 연출 재생
    private IEnumerator CoPlayRoutine()
    {
        // 시작 초기화
        ResetState();

        // 시작 딜레이
        yield return new WaitForSeconds(_startDelay);

        // 1차 퍼짐
        yield return CoAnimatePhase(
            Vector2.zero, _firstRedOffset,
            Vector2.zero, _firstBlueOffset,
            0f, _firstRotateZ,
            0f, _ghostAlpha,
            _firstSpreadDuration);

        // 유지
        yield return new WaitForSeconds(_firstHoldDuration);

        // 1차 복귀
        yield return CoAnimatePhase(
            _firstRedOffset, Vector2.zero,
            _firstBlueOffset, Vector2.zero,
            _firstRotateZ, 0f,
            _ghostAlpha, 0f,
            _firstReturnDuration);

        // 대기
        yield return new WaitForSeconds(_betweenDelay);

        // 2차 연출 전 초기화
        ForceStraightState();

        // 2차 퍼짐
        yield return CoAnimatePhase(
            Vector2.zero, _secondRedOffset,
            Vector2.zero, _secondBlueOffset,
            0f, 0f,
            0f, _ghostAlpha * 0.9f,
            _secondSpreadDuration);

        yield return new WaitForSeconds(_secondHoldDuration);

        // 2차 복귀
        yield return CoAnimatePhase(
            _secondRedOffset, Vector2.zero,
            _secondBlueOffset, Vector2.zero,
            0f, 0f,
            _ghostAlpha * 0.9f, 0f,
            _secondReturnDuration);

        // 끝나고 초기화
        ResetState();
        _playRoutine = null;
    }

    // 단계별 위치 / 회전 / 알파값 보간 (애니메이션)
    private IEnumerator CoAnimatePhase(
        Vector2 redFrom,
        Vector2 redTo,
        Vector2 blueFrom,
        Vector2 blueTo,
        float rotateFrom,
        float rotateTo,
        float alphaFrom,
        float alphaTo,
        float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            // 시간 증가
            time += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float t = Mathf.Clamp01(time / duration);
            // 부드러운 보간
            float eased = EaseInOutCubic(t);

            ApplyPhaseState(
                Vector2.Lerp(redFrom, redTo, eased),
                Vector2.Lerp(blueFrom, blueTo, eased),
                Mathf.Lerp(rotateFrom, rotateTo, eased),
                Mathf.Lerp(alphaFrom, alphaTo, eased));

            yield return null;
        }

        ApplyPhaseState(redTo, blueTo, rotateTo, alphaTo);
    }

    // 현재 단계의 연출 상태 적용
    private void ApplyPhaseState(Vector2 redOffset, Vector2 blueOffset, float zRotation, float alpha)
    {
        if (_mainLogo != null)
        {
            // 회전만 
            _mainLogo.localRotation = Quaternion.Euler(0f, 0f, zRotation);
        }

        if (_redLogo != null)
        {
            // 위치 이동
            _redLogo.anchoredPosition = _redOriginPosition + redOffset;
            _redLogo.localRotation = Quaternion.Euler(0f, 0f, zRotation);
        }

        if (_blueLogo != null)
        {
            _blueLogo.anchoredPosition = _blueOriginPosition + blueOffset;
            _blueLogo.localRotation = Quaternion.Euler(0f, 0f, zRotation);
        }

        // 잔상 알파
        SetImageAlpha(_redImage, alpha);
        SetImageAlpha(_blueImage, alpha);
    }

    // 공통 처리
    // 이미지 알파값 설정
    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null)
        {
            return;
        }

        Color color = image.color;
        // 투명도 설정
        color.a = alpha;
        image.color = color;
    }

    // 부드러운 움직임
    private float EaseInOutCubic(float time)
    {
        if (time < 0.5f)
        {
            return 4f * time * time * time;
        }

        return 1f - Mathf.Pow(-2f * time + 2f, 3f) / 2f;
    }

}