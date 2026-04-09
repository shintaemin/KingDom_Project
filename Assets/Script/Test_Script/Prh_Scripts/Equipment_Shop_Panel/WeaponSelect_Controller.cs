using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 슬롯 선택 관리
/*
 ▶ 할일
  - 아이쳄 슬롯 선택 상태를 관리
  - 잠금 상태가 아닌 슬롯만 선택 가능
  - 선택 시 다른 슬롯의 체크는 모두 해제

 ▶ 흐름
  1. 슬롯 클릭
  2. Lock / Check 오브젝트 확인
  3. 잠겨있으면 선택 불가
  4. 모든 슬롯의 체크 해제
  5. 클릭한 슬롯만 체크 활성화

- 박라희
*/
#endregion

public class WeaponSelect_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("슬롯 리스트")]
    [SerializeField] private List<GameObject> _weaponSlots;
    #endregion

    #region 내부 변수
    // 현재 선택된 슬롯
    private GameObject _currentSelectedSlot;
    #endregion

    // 클릭한 슬롯 선택 처리
    public void SelectSlot(GameObject clickedSlot)
    {
        if (clickedSlot == null)
        {
            return;
        }

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

        var slotData = clickedSlot.GetComponent<Equipment_Slot_Data>();

        if (slotData != null)
        {
            Debug.Log("선택된 장비 ID: " + slotData.GetData().ID);
        }

        // 모든 슬롯의 체크 해제
        ClearAllChecks();

        // 현재 슬롯만 체크 활성화
        checkObj.gameObject.SetActive(true);
        _currentSelectedSlot = clickedSlot;
    }

    // 현재 선택된 슬롯 반환
    public GameObject GetSelectedSlot()
    {
        return _currentSelectedSlot;
    }

    // 모든 슬롯의 체크 비활성화
    private void ClearAllChecks()
    {
        foreach (GameObject slot in _weaponSlots)
        {
            if (slot == null)
            {
                continue;
            }

            Transform checkObj = slot.transform.Find("Check");

            if (checkObj == null)
            {
                continue;
            }

            checkObj.gameObject.SetActive(false);
        }
    }
}
