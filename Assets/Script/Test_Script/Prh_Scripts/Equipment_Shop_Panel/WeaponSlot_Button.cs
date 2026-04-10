using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 아이템 슬롯 클릭 처리
/*
 ▶ 할일
  - 아이템 슬롯 버튼 클릭 이벤트 처리
  - 클릭된 슬롯을 WeaponSelect_Controller에 전달

 ▶ 흐름
  1. 슬롯 버튼 클릭 시 OnClickSlot() 호출
  2. controller가 존재하는지 확인
  3. 현재 슬롯(GameObject)을 컨트롤러에 전달

- 박라희
*/
#endregion

public class WeaponSlot_Button : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private WeaponSelect_Controller controller;
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
