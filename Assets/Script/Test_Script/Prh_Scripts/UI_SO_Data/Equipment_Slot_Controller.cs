using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 장비 슬롯 전체 제어
/*
 ▶ 할일
  - 슬롯에 데이터 초기화
  - 슬롯 클릭 시 선택 처리
  - 선택한 장비 데이터 저장

 ▶ 흐름
  1. Start에서 슬롯에 데이터 세팅
  2. 슬롯 클릭 시 기존 선택 해제
  3. 새 슬롯 선택 + 데이터 저장

  - 박라희
*/
#endregion

public class Equipment_Slot_Controller : MonoBehaviour
{/*
    #region 인스펙터
    [Header("슬롯 목록")]
    [SerializeField] private Equipment_Slot_Data[] slots;

    [Header("장비 ID 목록")]
    [SerializeField] private int[] ids;
    #endregion

    #region 내부 변수
    private Equipment_Slot_Data currentSelectedSlot;
    #endregion

    #region 초기화
    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i >= ids.Length)
            {
                Debug.LogWarning("ID 부족");
                break;
            }

            slots[i].SetData(ids[i]);
        }
    }
    #endregion

    #region 슬롯 클릭 처리 (선택만)
    public void OnClickSlot(Equipment_Slot_Data slot)
    {
        if (slot == null)
            return;

        // 기존 선택 해제
        if (currentSelectedSlot != null)
            currentSelectedSlot.SetSelected(false);

        // 새 선택
        currentSelectedSlot = slot;
        currentSelectedSlot.SetSelected(true);

        // 데이터 저장만
        Equipment_Data_Holder.Instance.currentEquipment = slot.GetData();

        Debug.Log("선택된 장비 ID: " + slot.GetData().ID);
    }
    #endregion
    */
}
