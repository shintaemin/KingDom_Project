using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion


public class Restart_Game : MonoBehaviour
{
    public void RestartInput()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SFXPlay(ESfxType.Touch_Button);
        }
        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.LoadScene(ESceneLoadType.TestGame);
        }
    }
    
}
