using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 옵션 체크
/*
 ▶ 할일
   - Start 에서 현재 옵션 상태를 확인하여 각 버튼에 On Off 지정
*/
#endregion


public class OptionCheck : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private LSettings_Toggle_Switch _bgmSwitch;
    [SerializeField] private LSettings_Toggle_Switch _sfxSwitch;
    [SerializeField] private LSettings_Quality _qualitySwitch;
    [SerializeField] private OpttionManager _om;
    #endregion

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            if (_sfxSwitch != null)
            {
                _sfxSwitch.SetOn(SoundManager.Instance.GetSfxToggle);
            }
            if (_bgmSwitch != null)
            {
                _bgmSwitch.SetOn(SoundManager.Instance.GetBgmToggle);
            }
        }

        if (_om != null && _qualitySwitch != null)
        {
            bool post = _om.GetUsePost;
            if (post)
            {
                _qualitySwitch.SelectHigh();
            }
            else
            {
                _qualitySwitch.SelectLow();
            }
        }
    }
}
