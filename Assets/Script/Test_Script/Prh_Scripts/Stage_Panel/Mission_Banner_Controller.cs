using System.Collections;
using UnityEngine;

#region 미션 배너 연출
/*
 ▶ 할일
  - 좌 / 우 UI 그룹이 중앙으로 이동하여 만나는 연출
  - 중앙에서 멈추지 않고 속도만 느려졌다가 다시 빨라지며 이동
  - 시작부터 끝까지 한 번의 흐름으로 자연스럽게 처리

  - 박라희
 */
#endregion

public class Mission_Banner_Controller : MonoBehaviour
{
    #region 인스펙터

    [Header("참조")]
    [SerializeField] private RectTransform _leftRect;
    [SerializeField] private RectTransform _rightRect;

    [Header("위치")]
    [SerializeField] private Vector2 _leftStart;
    [SerializeField] private Vector2 _leftMeet;

    [SerializeField] private Vector2 _rightStart;
    [SerializeField] private Vector2 _rightMeet;

    [Header("끝 이동 거리")]
    [SerializeField] private float _outDistance = 1400f;

    [Header("시간 설정")]
    [SerializeField] private float _approachTime = 0.35f;
    [SerializeField] private float _slowTime = 3.0f;
    [SerializeField] private float _exitTime = 0.45f;

    [Header("슬로우 이동 거리")]
    [SerializeField] private float _slowMoveDistance = 80f;
    #endregion

    #region 내부 변수
    // 현재 실행 중인 코루틴 저장
    private Coroutine _playCoroutine;
    #endregion

    private void Start()
    {
        // 기존 코루틴이 있으면 중지
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
        }

        // 배너 연출 시작
        _playCoroutine = StartCoroutine(CoPlayBanner());
    }

    #region 코루틴
    // 전체 배너 연출 흐름
    private IEnumerator CoPlayBanner()
    {
        yield return new WaitForSeconds(0f);

        // 시작 위치 세팅
        _leftRect.anchoredPosition = _leftStart;
        _rightRect.anchoredPosition = _rightStart;

        // 이동 방향 계산
        Vector2 leftDir = (_leftMeet - _leftStart).normalized;
        Vector2 rightDir = (_rightMeet - _rightStart).normalized;

        // 중앙 이후 천천히 이동할 위치 계산
        Vector2 leftSlowEnd = _leftMeet + leftDir * _slowMoveDistance;
        Vector2 rightSlowEnd = _rightMeet + rightDir * _slowMoveDistance;

        // 최종 화면 밖 위치 계산
        Vector2 leftEnd = leftSlowEnd + leftDir * _outDistance;
        Vector2 rightEnd = rightSlowEnd + rightDir * _outDistance;

        // 1. 시작 → 중앙 빠르게 이동
        yield return CoMove(
            _leftRect,
            _rightRect,
            _leftStart,
            _rightStart,
            _leftMeet,
            _rightMeet,
            _approachTime);

        // 2. 중앙 → 슬로우 이동
        yield return CoMove(
            _leftRect,
            _rightRect,
            _leftMeet,
            _rightMeet,
            leftSlowEnd,
            rightSlowEnd,
            _slowTime);

        // 3. 슬로우 → 화면 밖 빠르게 이동
        yield return CoMove(
            _leftRect,
            _rightRect,
            leftSlowEnd,
            rightSlowEnd,
            leftEnd,
            rightEnd,
            _exitTime);

        // 코루틴 종료 표시
        _playCoroutine = null;
    }

    // 좌 / 우 배너 동시에 이동
    private IEnumerator CoMove(
        RectTransform leftRect,
        RectTransform rightRect,
        Vector2 leftFrom,
        Vector2 rightFrom,
        Vector2 leftTo,
        Vector2 rightTo,
        float moveTime)
    {
        // 경과 시간
        float elapsed = 0f;

        while (elapsed < moveTime)
        {
            // 시간 증가
            elapsed += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float t = Mathf.Clamp01(elapsed / moveTime);

            // 부드러운 보간
            float easedT = t * t * (3f - 2f * t);

            // 위치 보간 적용
            leftRect.anchoredPosition = Vector2.Lerp(leftFrom, leftTo, easedT);
            rightRect.anchoredPosition = Vector2.Lerp(rightFrom, rightTo, easedT);

            // 다음 프레임 대기
            yield return null;
        }

        // 마지막 정확한 위치 보정
        leftRect.anchoredPosition = leftTo;
        rightRect.anchoredPosition = rightTo;
    }
    #endregion
}