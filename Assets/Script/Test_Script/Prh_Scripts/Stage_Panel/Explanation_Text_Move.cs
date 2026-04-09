using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 ▶ 할일
  - 왼쪽 화면 밖에서 시작
  - 가운데로 이동하면서 점점 선명해짐
  - 가운데에서 1초 대기
  - 오른쪽으로 이동하면서 다시 흐려지며 사라짐

 ▶ 흐름
  1. 시작 위치 + 흐릿한 알파 적용
  2. 왼쪽 -> 중앙 이동 + 알파 증가
  3. 중앙에서 1초 정지
  4. 중앙 -> 오른쪽 이동 + 알파 감소
*/


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

    private Coroutine _playCoroutine;

    private void OnEnable()
    {
        if (_playCoroutine != null)
            StopCoroutine(_playCoroutine);

        _playCoroutine = StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        // 시작 위치 / 시작 투명도
        _textRect.anchoredPosition = _startPos;
        SetAlpha(_startAlpha);

        // 왼쪽 -> 중앙 이동 + 점점 진하게
        yield return MoveAndFade(_startPos, _centerPos, _startAlpha, _centerAlpha, _moveToCenterDuration);

        // 중앙에서 잠깐 멈춤
        yield return new WaitForSeconds(_waitTime);

        // 중앙 -> 오른쪽 이동 + 점점 흐리게
        yield return MoveAndFade(_centerPos, _endPos, _centerAlpha, _endAlpha, _moveToEndDuration);
    }

    private IEnumerator MoveAndFade(Vector2 startPos, Vector2 endPos, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            t = Mathf.Clamp01(t);

            // 부드러운 움직임
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            _textRect.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothT);

            float alpha = Mathf.Lerp(startAlpha, endAlpha, smoothT);
            SetAlpha(alpha);

            yield return null;
        }

        _textRect.anchoredPosition = endPos;
        SetAlpha(endAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (_text == null)
            return;

        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
    }
}
