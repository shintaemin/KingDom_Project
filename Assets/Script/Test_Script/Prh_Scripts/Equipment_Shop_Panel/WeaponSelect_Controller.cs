using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 무기 선택 관리
/*
 ▶ 할일
  - 무기 슬롯 클릭 시 선택 처리
  - 선택된 슬롯의 체크 표시 활성화
  - 기존 선택 상태 초기화
  - 플레이어 데이터에 선택된 무기 ID 저장

 ※ 참고사항
  - 잠금 상태인 슬롯은 선택 불가
  - CPlayerDataManager 초기화 이후 선택 상태 적용
  - 슬롯 내부의 Lock / Check / Open 오브젝트를 통해 상태 표현

   - 박라희
*/
#endregion
public class WeaponSelect_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("전체 무기 슬롯")]
    [SerializeField] private List<GameObject> _allWeaponSlots;
    #endregion

    #region 내부 변수
    // 현재 선택된 슬롯
    private GameObject _currentSelectedSlot;
    #endregion

    private void Start()
    {
        // 데이터 로드 이후 초기화
        StartCoroutine(CoInitAfterLoad());
    }

    #region 내부 코루틴
    // PlayerDataManager 초기화 이후 실행
    private IEnumerator CoInitAfterLoad()
    {
        yield return new WaitUntil(() => CPlayerDataManager.Instance != null);

        InitSelectedWeapon();
    }
    #endregion

    #region 외부 호출 함수
    // 슬롯 선택 처리
    public void SelectSlot(GameObject clickedSlot)
    {
        // 유효성 검사
        if (clickedSlot == null)
            return;

        Transform lockTr = clickedSlot.transform.Find("Lock");
        Transform checkTr = clickedSlot.transform.Find("Check");

        if (lockTr == null || checkTr == null)
            return;

        // 잠긴 슬롯은 선택 불가
        if (lockTr.gameObject.activeSelf)
        {
            Debug.Log("잠긴 아이템은 선택 불가");
            return;
        }

        var slotData = clickedSlot.GetComponent<Equipment_Slot_Data>();

        if (slotData != null)
        {
            int id = slotData.GetData().ID;

            // 플레이어 데이터에 선택된 무기 ID 저장
            CPlayerDataManager.Instance.CurrentWeaponID = id;

            Debug.Log("선택된 무기 ID: " + id);
        }

        // 기존 선택 해제
        ClearAllChecks();

        // 현재 선택 표시
        checkTr.gameObject.SetActive(true);
        _currentSelectedSlot = clickedSlot;
    }
    #endregion

    #region 내부 함수
    // 모든 슬롯 체크 상태 초기화
    private void ClearAllChecks()
    {
        foreach (GameObject slot in _allWeaponSlots)
        {
            if (slot == null)
                continue;

            Transform checkTr = slot.transform.Find("Check");

            if (checkTr != null)
                checkTr.gameObject.SetActive(false);
        }
    }

    // 현재 장착된 무기 기준으로 초기 선택 적용
    private void InitSelectedWeapon()
    {
        Debug.Log("InitSelectedWeapon 실행됨");

        int currentID = CPlayerDataManager.Instance.CurrentWeaponID;

        foreach (GameObject slot in _allWeaponSlots)
        {
            if (slot == null) continue;

            var data = slot.GetComponent<Equipment_Slot_Data>();
            if (data == null) continue;
            if (data.GetData() == null) continue;

            // 현재 장착 무기 찾기
            if (data.GetData().ID == currentID)
            {
                Transform lockTr = slot.transform.Find("Lock");
                Transform openTr = slot.transform.Find("Open");
                Transform checkTr = slot.transform.Find("Check");

                // 잠금 해제 상태 적용
                if (lockTr != null) lockTr.gameObject.SetActive(false);
                if (openTr != null) openTr.gameObject.SetActive(true);

                // 선택 상태 적용
                ClearAllChecks();
                SelectSlot(slot);

                break;
            }
        }
    }
    #endregion
}