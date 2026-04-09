using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPopup_Controller : MonoBehaviour
{
    [SerializeField] private GameObject _popup;

    public void OnClickReceive()
    {
        if (_popup != null)
        {
            _popup.SetActive(false);
        }
    }
}
