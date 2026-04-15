using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 인게임 배너 레벨 텍스트 페이드
/*
 ▶ 할일
  - 텍스트가 서서히 나타남
  - 잠시 대기
  - 텍스트가 서서히 사라짐

 - 박라희
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

        // 페이드 연출 시작
        _playCoroutine = StartCoroutine(CoPlayFade());
    }

    #region 내부 코루틴
    private IEnumerator CoPlayFade()
    {
        // 시작 시 투명하게 설정
        SetAlpha(_startAlpha);

        // 1. Fade In (등장)
        yield return CoFade(_startAlpha, _maxAlpha, _fadeInDuration);

        // 2. 잠시 유지
        yield return new WaitForSeconds(_stayDuration);

        // 3. Fade Out (퇴장)
        yield return CoFade(_maxAlpha, _endAlpha, _fadeOutDuration);

        _playCoroutine = null;
    }

    // 알파값을 시간에 따라 부드럽게 변화시키는 코루틴
    private IEnumerator CoFade(float fromAlpha, float toAlpha, float duration)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            // 시간 누적
            currentTime += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float t = Mathf.Clamp01(currentTime / duration);

            // 부드러운 변화 (ease-in/out)
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // 알파 보간
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, smoothT);
            SetAlpha(alpha);

            yield return null;
        }

        // 마지막 값 보정
        SetAlpha(toAlpha);
    }
    #endregion

    #region 내부 함수
    // 텍스트 알파값 적용
    private void SetAlpha(float alpha)
    {
        if (_levelText == null)
            return;

        Color color = _levelText.color;
        color.a = alpha;
        _levelText.color = color;
    }
    #endregion
}
