using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region

#endregion


public class GemUIUpdate : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TextMeshProUGUI _gemText;
    #endregion

    #region

    #endregion

    private void Start()
    {
        GemTextUpdate();
    }

    #region 외부 호출 함수
    public void GemTextUpdate()
    {
        if (CPlayerDataManager.Instance != null)
        {
            int gem = CPlayerDataManager.Instance.Gem;
            _gemText.text = gem >= 100000 ? "99999" : $"{gem}";
        }
    }
    #endregion
}
