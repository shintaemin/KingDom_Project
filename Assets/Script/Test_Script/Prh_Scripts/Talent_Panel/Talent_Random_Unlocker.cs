using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 재능 랜덤 해금 처리
/*
 ▶ 할일
  - 잠겨있는 재능 슬롯 중 하나를 랜덤으로 선택하여 해금
  - 선택된 슬롯의 Lock → Open 상태로 변경

 ※ 참고사항
  - 잠긴 슬롯만 대상으로 처리
  - 해금 가능한 슬롯이 없으면 실행 중단

  - 박라희
*/
#endregion

public class Talent_Random_Unlocker : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Talent_Select_Controller _controller;

    [Header("슬롯 리스트")]
    [SerializeField] private List<GameObject> _talentSlots = new List<GameObject>();
    #endregion

    #region 외부 호출 함수
    // 랜덤 재능 해금
    public void UnlockRandomTalent()
    {
        List<GameObject> lockedSlots = new List<GameObject>();

        // 잠긴 슬롯 수집
        foreach (GameObject slot in _talentSlots)
        {
            if (slot == null)
                continue;

            Transform tr = slot.transform;
            GameObject lockObj = tr.Find("Lock").gameObject;

            if (lockObj.activeSelf)
                lockedSlots.Add(slot);
        }

        // 해금 가능한 슬롯 없으면 종료
        if (lockedSlots.Count == 0)
        {
            Debug.Log("열 수 있는 슬롯 없음");
            return;
        }

        // 랜덤 슬롯 선택
        int randomIndex = Random.Range(0, lockedSlots.Count);
        GameObject selectedSlot = lockedSlots[randomIndex];

        // 선택된 슬롯 해금 처리
        Transform trSelected = selectedSlot.transform;
        trSelected.Find("Lock").gameObject.SetActive(false);
        trSelected.Find("Open").gameObject.SetActive(true);

        // 슬롯 인덱스 확인
        int index = _talentSlots.IndexOf(selectedSlot);

        Debug.Log($"열린 슬롯: {selectedSlot.name}");
    }
    #endregion
}
