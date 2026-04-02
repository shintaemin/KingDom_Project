using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region

#endregion


public class Ingame_KillCount_UI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TextMeshProUGUI _killText;
    #endregion

    #region 외부 호출 함수
    public void SetKillCountTextUpdata(int remain, int target)
    {
        if (_killText == null)
        {
            return;
        }

        _killText.text = $"{remain}/{target}";
        Debug.Log($"현재 {remain} , 목표 {target}");
    }
    #endregion
}
