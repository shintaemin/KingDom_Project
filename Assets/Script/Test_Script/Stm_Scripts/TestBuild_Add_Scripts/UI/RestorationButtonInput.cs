using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region

#endregion


public class RestorationButtonInput : MonoBehaviour
{
#region

#endregion

#region

#endregion

    public void RestorationInput()
    {
        if (CJsonManager.Instance != null)
        {
            CJsonManager.Instance.RessetStartInput();
        }
    }
}
