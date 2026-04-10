using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesSlot_Button : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private ClothesSelect_Controller controller;
    #endregion

    public void OnClickSlot()
    {
        // 컨트롤러가 없으면 동작 중단
        if (controller == null) 
            return;

        // 현재 슬롯을 선택 요청
        controller.SelectSlot(gameObject);
    }
}
