using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion


public class Reward_Button_Ad_Input : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private int _addGemCount = 500;
#endregion

#region 외부 호출 함수
    public void AddGem()
    {
        if(CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.Gem = _addGemCount;
        }
    }

    public void StarterPackRewarde()
    {
        if (CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.Gem = 4000;
            CPlayerDataManager.Instance.Energy = 40;
        }
    }
#endregion

    
}
