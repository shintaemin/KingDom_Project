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
    private GameObject _currentSelectedSlot;
    #endregion

    void Start()
    {
        StartCoroutine(InitAfterLoad());
    }

    IEnumerator InitAfterLoad()
    {
        yield return new WaitUntil(() => CPlayerDataManager.Instance != null);

        InitSelectedClothes();
    }

    
    // 클릭한 슬롯 선택 처리
    public void SelectSlot(GameObject clickedSlot)
    {
        if (clickedSlot == null)
            return;

        Transform lockObj = clickedSlot.transform.Find("Lock");
        Transform checkObj = clickedSlot.transform.Find("Check");

        if (lockObj == null || checkObj == null)
            return;

        if (lockObj.gameObject.activeSelf)
        {
            Debug.Log("잠긴 아이템은 선택 불가");
            return;
        }

        var slotData = clickedSlot.GetComponent<Equipment_Slot_Data>();

        if (slotData != null)
        {
            int id = slotData.GetData().ID;

            CPlayerDataManager.Instance.CurrentClothesID = id;

            Debug.Log("선택된 옷 ID: " + id);
        }

        ClearAllChecks();

        checkObj.gameObject.SetActive(true);
        _currentSelectedSlot = clickedSlot;
    }

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

    void InitSelectedClothes()
    {
        Debug.Log("InitSelectedClothes 실행됨");

        int currentID = CPlayerDataManager.Instance.CurrentClothesID;

        foreach (GameObject slot in _allClothesSlots)
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