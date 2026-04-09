using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 레벨 텍스트 페이드 연출
/*
 ▶ 할일
  - 텍스트가 서서히 나타남
  - 잠시 유지
  - 텍스트가 서서히 사라짐

 ▶ 흐름
  1. 시작 알파값 적용
  2. 점점 진해지며 등장
  3. 잠시 유지
  4. 점점 흐려지며 사라짐
*/
#endregion

public class LevelText_Fade_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("참조")]
    [SerializeField] private TextMeshProUGUI _levelText;

    [Header("시간")]
    [SerializeField] private float _fadeInDuration = 0.8f;
    [SerializeField] private float _stayDuration = 0.8f;
    [SerializeField] private float _fadeOutDuration = 0.8f;

    [Header("알파값")]
    [SerializeField, Range(0f, 1f)] private float _startAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float _maxAlpha = 1f;
    [SerializeField, Range(0f, 1f)] private float _endAlpha = 0f;
    #endregion

    #region 내부 변수
    private Coroutine _playCoroutine;
    #endregion

    private void OnEnable()
    {
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
        }

        _playCoroutine = StartCoroutine(CoPlayFade());
    }

    private IEnumerator CoPlayFade()
    {
        // 시작 시 투명하게 설정
        SetAlpha(_startAlpha);

        // 1. 서서히 나타남
        yield return CoFade(_startAlpha, _maxAlpha, _fadeInDuration);

        // 2. 잠시 유지
        yield return new WaitForSeconds(_stayDuration);

        // 3. 서서히 사라짐
        yield return CoFade(_maxAlpha, _endAlpha, _fadeOutDuration);

        _playCoroutine = null;
    }

    private IEnumerator CoFade(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            float alpha = Mathf.Lerp(fromAlpha, toAlpha, smoothT);
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(toAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (_levelText == null)
            return;

        Color color = _levelText.color;
        color.a = alpha;
        _levelText.color = color;
    }
}
