using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesSelect_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("전체 옷 슬롯")]
    [SerializeField] private List<GameObject> _allClothesSlots;
    #endregion

    #region 내부 변수
    // 현재 선택된 슬롯
    private GameObject _currentSelectedSlot;
    #endregion

    // 클릭한 슬롯 선택 처리
    public void SelectSlot(GameObject clickedSlot)
    {
        if (clickedSlot == null)
            return;

        Transform lockObj = clickedSlot.transform.Find("Lock");
        Transform checkObj = clickedSlot.transform.Find("Check");

        if (lockObj == null || checkObj == null)
            return;

        // 잠겨있으면 선택 불가
        if (lockObj.gameObject.activeSelf)
        {
            Debug.Log("잠긴 아이템은 선택 불가");
            return;
        }

        // 데이터 저장 (옷)
        var slotData = clickedSlot.GetComponent<Equipment_Slot_Data>();
        if (slotData != null && Equipment_Data_Holder.Instance != null)
        {
            Equipment_Data_Holder.Instance.currentClothes = slotData.GetData();
            Debug.Log("저장된 장비 ID: " + slotData.GetData().ID);
        }

        // 기존 선택 체크 해제
        ClearAllChecks();

        // 현재 슬롯만 체크 활성화
        checkObj.gameObject.SetActive(true);
        _currentSelectedSlot = clickedSlot;
    }

    /*
    // 현재 선택된 슬롯 반환
    public GameObject GetSelectedSlot()
    {
        return _currentSelectedSlot;
    }
    */


    // 모든 슬롯의 체크 비활성화
    private void ClearAllChecks()
    {
        foreach (GameObject slot in _allClothesSlots)
        {
            if (slot == null)
                continue;

            Transform checkObj = slot.transform.Find("Check");

            if (checkObj != null)
                checkObj.gameObject.SetActive(false);
        }
    }
}
