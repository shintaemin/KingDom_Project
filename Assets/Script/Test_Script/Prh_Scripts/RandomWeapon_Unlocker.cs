using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 아이템 슬롯 랜덤 해금 관리
/*
 ▶ 할일
  - 잠겨있는 아이템 중 하나를 랜덤으로 선택하여 해금
  - Lock → Open 상태로 변경
  - 모든 아이템이 해금된 경우 동작하지 않음

 ▶ 흐름
  1. 모든 슬롯 검사
  2. 잠긴 슬롯만 리스트에 수집
  3. 랜덤으로 하나 선택
  4. 선택된 슬롯을 해금 상태로 변경
*/
#endregion

public class RandomWeapon_Unlocker : MonoBehaviour
{
    #region 인스펙터
    [Header("아이템 목록")]
    [SerializeField] private List<GameObject> _weaponSlots;
    #endregion

    // 잠겨있는 아이템 중 랜덤으로 하나 해금
    public void UnlockRandomWeapon()
    {
        List<GameObject> lockedSlots = new List<GameObject>();

        // 잠긴 슬롯 찾기
        foreach (GameObject slot in _weaponSlots)
        {
            if (slot == null) continue;

            Transform lockObj = slot.transform.Find("Lock");
            Transform openObj = slot.transform.Find("Open");

            if (lockObj == null || openObj == null) continue;

            // Lock 상태면 잠긴 슬롯으로 판단
            if (lockObj.gameObject.activeSelf)
            {
                lockedSlots.Add(slot);
            }
        }

        // 전부 해금된 경우
        if (lockedSlots.Count == 0)
        {
            return;
        }

        // 랜덤 선택
        GameObject selectedSlot = lockedSlots[Random.Range(0, lockedSlots.Count)];

        // 선택된 슬롯 해금 처리
        SetUnlock(selectedSlot);
    }
   
    // 슬롯을 해금 상태로 변경 (Lock OFF / Open ON)
    private void SetUnlock(GameObject slot)
    {
        Transform lockObj = slot.transform.Find("Lock");
        Transform openObj = slot.transform.Find("Open");

        if (lockObj != null) lockObj.gameObject.SetActive(false);
        if (openObj != null) openObj.gameObject.SetActive(true);
    }
}
