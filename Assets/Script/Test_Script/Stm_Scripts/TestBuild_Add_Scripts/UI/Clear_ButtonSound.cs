using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion


public class Clear_ButtonSound : MonoBehaviour
{
#region

#endregion

#region

#endregion

    public void ButtonSoundPlay()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SFXPlay(ESfxType.Touch_Button);
        }
    }
}
