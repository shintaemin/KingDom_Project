using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 인게임 배너 설명 텍스트 이동 연출
/*
 ▶ 할일
  - 텍스트를 화면 왼쪽 밖에서 시작하여 중앙으로 이동
  - 이동하면서 점점 선명해지고, 중앙에서 잠시 유지
  - 이후 오른쪽으로 이동하며 다시 흐려지면서 사라짐

 ▶ 흐름
  1. 시작 위치 + 낮은 알파 적용 (화면 밖)
  2. 왼쪽 → 중앙 이동 + 알파 증가 (등장)
  3. 중앙에서 대기
  4. 중앙 → 오른쪽 이동 + 알파 감소 (퇴장)

 ※ 참고사항
  - SmoothStep을 사용하여 부드러운 가속/감속 이동 적용
  - Lerp를 사용하여 위치와 알파를 동시에 보간

  - 박라희
*/
#endregion

public class Explanation_Text_Move : MonoBehaviour
{
    #region 인스펙터
    [Header("참조")]
    [SerializeField] private RectTransform _textRect;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("위치")]
    [SerializeField] private Vector2 _startPos = new Vector2(-1200f, 0f);
    [SerializeField] private Vector2 _centerPos = new Vector2(0f, 0f);
    [SerializeField] private Vector2 _endPos = new Vector2(1200f, 0f);

    [Header("시간")]
    [SerializeField] private float _moveToCenterDuration = 0.8f;
    [SerializeField] private float _waitTime = 1.0f;
    [SerializeField] private float _moveToEndDuration = 0.8f;

    [Header("알파값")]
    [SerializeField, Range(0f, 1f)] private float _startAlpha = 0.25f;
    [SerializeField, Range(0f, 1f)] private float _centerAlpha = 1f;
    [SerializeField, Range(0f, 1f)] private float _endAlpha = 0f;
    #endregion

    #region 내부 변수
    private Coroutine _playCoroutine;
    #endregion

    private void OnEnable()
    {
        if (_playCoroutine != null)
            StopCoroutine(_playCoroutine);

        // 연출 시작
        _playCoroutine = StartCoroutine(CoPlay());
    }

    #region 내부 코루틴
    // 전체 연출 흐름 제어
    private IEnumerator CoPlay()
    {
        // 시작 상태 설정 (위치 + 투명도)
        _textRect.anchoredPosition = _startPos;
        SetAlpha(_startAlpha);

        // 왼쪽 → 중앙 이동 + 페이드 인
        yield return CoMoveAndFade(_startPos, _centerPos, _startAlpha, _centerAlpha, _moveToCenterDuration);

        // 중앙에서 대기
        yield return new WaitForSeconds(_waitTime);

        // 중앙 → 오른쪽 이동 + 페이드 아웃
        yield return CoMoveAndFade(_centerPos, _endPos, _centerAlpha, _endAlpha, _moveToEndDuration);
    }

    // 위치 이동 + 알파 변화 동시 처리
    private IEnumerator CoMoveAndFade(Vector2 startPos, Vector2 endPos, float startAlpha, float endAlpha, float duration)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            // 시간 누적
            currentTime += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float t = Mathf.Clamp01(currentTime / duration);

            // 자연스러운 in/out 효과
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // 위치
            _textRect.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothT);

            // 알파
            float alpha = Mathf.Lerp(startAlpha, endAlpha, smoothT);
            SetAlpha(alpha);

            yield return null;
        }

        // 최종 값 보정
        _textRect.anchoredPosition = endPos;
        SetAlpha(endAlpha);
    }
    #endregion

    #region 내부 함수
    // 텍스트 알파값 적용
    private void SetAlpha(float alpha)
    {
        if (_text == null)
            return;

        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
    }
    #endregion
}
