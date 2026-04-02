using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 페이드 시스템
/*
 ▶ 할일
  - 비동기 Fade 시스템을 담당

    - 작업자 : 신태민 - 
*/
#endregion

public class FadeSystem : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private FadeUI _fadeUI;
    #endregion

    #region 내부변수
    private Coroutine _fadeCo;
    #endregion

    private void Awake()
    {
        if (_fadeUI == null)
        {
            if (!transform.GetChild(0).TryGetComponent<FadeUI>(out _fadeUI))
            {
                Debug.LogWarning($"[FadeSystem] : 페이드 UI 참조 실패");
                return;
            }
        }

        _fadeCo = null;
    }

    private void FadeSettingReset()
    {
        if (_fadeCo == null)
        {
            return;
        }

        StopCoroutine(_fadeCo);
        _fadeCo = null;
    }

    private IEnumerator CoFadeSysyem(float to, float from, float time)
    {
        if (_fadeUI == null)
        {
            _fadeCo = null;
            yield break;
        }

        float remain = 0;
        float start = to;
        float end = from;

        while (remain < time)
        {
            remain += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(remain / time);
            float alpha = Mathf.Lerp(start, end, progress);

            _fadeUI.SetFade(alpha);

            yield return null;
        }

        _fadeUI.SetFade(end);
        _fadeCo = null;
        yield return null;
    }

    #region 외부 호출 함수
    public void Fade(float to, float from, float time = 2f)
    {
        FadeSettingReset();
        _fadeCo = StartCoroutine(CoFadeSysyem(to, from, time));
    }

    public void SetActiveFade(bool active)
    {
        _fadeUI.SetActive(active);
    }
    #endregion
}
