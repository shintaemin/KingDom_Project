using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory_Panel_Controller : MonoBehaviour
{
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

    private void Awake()
    {
        // 캐릭터 제외 나머지 전부 숨김
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

    private IEnumerator PlayIntro()
    {
        // 1. 캐릭터만 보여주는 시간
        yield return new WaitForSeconds(_characterWaitTime);

        // 2. 나머지 UI 동시에 등장
        StartCoroutine(Fade(_bg, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_impact, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_levelText, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_victoryText, 0f, 1f, _fadeTime));
        StartCoroutine(Fade(_button, 0f, 1f, _fadeTime));
    }

    private void SetAlpha(CanvasGroup target, float alpha)
    {
        if (target == null)
            return;

        target.alpha = alpha;
    }

    private IEnumerator Fade(CanvasGroup target, float start, float end, float duration)
    {
        if (target == null)
            yield break;

        float time = 0f;
        target.alpha = start;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            target.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }

        target.alpha = end;
    }
}
