using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Button_Sound : MonoBehaviour
{
    public void PlaySound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SFXPlay(ESfxType.Touch_Button);
        }
    }
}
