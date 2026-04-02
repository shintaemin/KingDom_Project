using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Fade UI
/*
 ▶ 할일
  - Fade UI On Off 를 관리
  - Fade 를 진행할수 있도록 이미지의 Alpha 값을 수정할 수 있도록 작업

    - 작업자 : 신태민 - 
*/
#endregion


public class FadeUI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Image _fadeImage;
    [SerializeField] private bool _active;
    #endregion

    #region 외부 호출 함수
    public bool SetActive(bool active)
    {
        if (_active == active) return false;

        _active = active;
        _fadeImage.gameObject.SetActive(_active);
        return true;
    }

    public void SetFade(float target)
    {
        if (!_active)
        {
            return;
        }

        target = Mathf.Clamp01(target);
        Color col = _fadeImage.color;
        col.a = target;
        _fadeImage.color = col;
    }

    public bool GetActive => _active;
    #endregion
}
