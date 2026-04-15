using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 승리 패널 연출
/*
 ▶ 할일
  - 전투 종료 후 승리 UI를 순차적으로 자연스럽게 등장시킴
  - 캐릭터 연출 이후 나머지 UI를 동시에 페이드 인

 ▶ 흐름
  1. 시작 시 캐릭터 제외 모든 UI 투명 처리
  2. 일정 시간 동안 캐릭터만 노출
  3. 이후 모든 UI를 동시에 페이드 인

 ※ 참고사항
  - CanvasGroup을 사용하여 UI 전체 알파 제어

  - 박라희
*/
#endregion

public class Victory_Panel_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("등장 대상")]
    [SerializeField] private CanvasGroup _bg;
    [SerializeField] private CanvasGroup _impact;
    [SerializeField] private CanvasGroup _levelText;
    [SerializeField] private CanvasGroup _victoryText;
    [SerializeField] private CanvasGroup _button;

    [Header("캐릭터 대기 시간")]
    [SerializeField] private float _characterWaitTime = 3f;

    [Header("등장 속도")]
    [SerializeField] private float _fadeTime = 1.5f;
    #endregion

    private void Awake()
    {
        // 초기 상태: 캐릭터 제외 UI 전부 숨김 (알파 0)
        SetAlpha(_bg, 0f);
        SetAlpha(_impact, 0f);
        SetAlpha(_levelText, 0f);
        SetAlpha(_victoryText, 0f);
        SetAlpha(_button, 0f);
    }

    private void OnEnable()
    {
        StartCoroutine(PlayIntro());
    }

    #region 코루틴
    private IEnumerator PlayIntro()
    {
        // 1. 캐릭터만 보여주는 구간
        yield return new WaitForSeconds(_characterWaitTime);

        // 2. 나머지 UI 동시에 등장
        StartCoroutine(Fade(_bg, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_impact, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_levelText, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_victoryText, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_button, 0f, 1f, _fadeTime));
    }

    // CanvasGroup 알파값 페이드 처리
    private IEnumerator Fade(CanvasGroup target, float start, float end, float duration)
    {
        if (target == null)
            yield break;

        float currentTime = 0f;

        // 시작 알파 설정
        target.alpha = start;

        while (currentTime < duration)
        {
            // 시간 누적
            currentTime += Time.deltaTime;
            // 진행률 계산
            float t = Mathf.Clamp01(currentTime / duration);
            // 알파 보간
            target.alpha = Mathf.Lerp(start, end, t);

            yield return null;
        }

        // 최종 값 보정
        target.alpha = end;
    }
    #endregion

    #region 내부 함수
    // 즉시 알파값 설정 (초기화용)
    private void SetAlpha(CanvasGroup target, float alpha)
    {
        if (target == null)
            return;

        target.alpha = alpha;
    }
    #endregion
}
