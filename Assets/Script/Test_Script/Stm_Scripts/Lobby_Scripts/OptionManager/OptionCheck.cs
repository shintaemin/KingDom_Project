using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion


public class OptionCheck : MonoBehaviour
{
    #region ¿ŒΩ∫∆Â≈Õ
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
