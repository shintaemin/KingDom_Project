using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("전체 무기 슬롯")]
    [SerializeField] private List<GameObject> _allWeaponSlots;
    #endregion

    #region 내부 변수
    private GameObject _currentSelectedSlot;
    #endregion

    void Start()
    {
        StartCoroutine(InitAfterLoad());
    }

    IEnumerator InitAfterLoad()
    {
        yield return new WaitUntil(() => CPlayerDataManager.Instance != null);

        InitSelectedWeapon();
    }


    // 슬롯 클릭
    public void SelectSlot(GameObject clickedSlot)
    {
        if (clickedSlot == null) 
            return;

        Transform lockObj = clickedSlot.transform.Find("Lock");
        Transform checkObj = clickedSlot.transform.Find("Check");

        if (lockObj == null || checkObj == null) 
            return;

        // 잠금 상태 체크
        if (lockObj.gameObject.activeSelf)
        {
            Debug.Log("잠긴 아이템은 선택 불가");
            return;
        }

        var slotData = clickedSlot.GetComponent<Equipment_Slot_Data>();

        if (slotData != null)
        {
            int id = slotData.GetData().ID;

            // 데이터 세팅
            CPlayerDataManager.Instance.CurrentWeaponID = id;

            Debug.Log("선택된 무기 ID: " + id);
        }

        ClearAllChecks();

        checkObj.gameObject.SetActive(true);
        _currentSelectedSlot = clickedSlot;
    }

    private void ClearAllChecks()
    {
        foreach (GameObject slot in _allWeaponSlots)
        {
            if (slot == null) 
                continue;

            Transform checkObj = slot.transform.Find("Check");

            if (checkObj != null)
                checkObj.gameObject.SetActive(false);
        }
    }

    void InitSelectedWeapon()
    {
        Debug.Log("InitSelectedWeapon 실행됨");

        int currentID = CPlayerDataManager.Instance.CurrentWeaponID;

        foreach (GameObject slot in _allWeaponSlots)
        {
            if (slot == null) continue;

            var data = slot.GetComponent<Equipment_Slot_Data>();
            if (data == null) continue;
            if (data.GetData() == null) continue;

            if (data.GetData().ID == currentID)
            {
                Transform lockObj = slot.transform.Find("Lock");
                Transform openObj = slot.transform.Find("Open");
                Transform checkObj = slot.transform.Find("Check");

                // 잠금 해제
                if (lockObj != null) lockObj.gameObject.SetActive(false);
                if (openObj != null) openObj.gameObject.SetActive(true);

                ClearAllChecks();
                SelectSlot(slot);

                break;
            }
        }
    }
}