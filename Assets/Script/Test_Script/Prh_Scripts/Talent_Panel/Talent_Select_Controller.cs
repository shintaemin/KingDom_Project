using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 재능 선택 및 해금 관리
/*
 ▶ 할일
  - 재능 슬롯의 잠금 / 해금 상태 관리
  - 랜덤 해금 연출(하이라이트 + 사운드) 처리
  - 선택된 슬롯 체크 표시 연출 처리
  - 해금 시 팝업 UI 활성화

 ▶ 흐름
  1. 시작 시 슬롯 상태 초기화 (InitSlots)
  2. 중앙 슬롯 기본 해금 처리
  3. 랜덤 해금 요청 시
     - 잠긴 슬롯 수집
     - 마지막 1개면 즉시 처리
     - 아니면 룰렛 연출 진행
  4. 최종 슬롯 해금 및 팝업 실행
  5. 슬롯 선택 시 체크 연출 적용

 ※ 참고사항
  - 슬롯 상태는 unlockedStates 배열로 관리
  - Highlight / Check / Lock / Open 오브젝트로 UI 상태 표현
  - 코루틴을 이용한 연출 처리

  - 박라희
*/
#endregion

public class Talent_Select_Controller : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private List<Talent_Slot_Popup> _popups;

    [Header("슬롯 리스트")]
    [SerializeField] private List<GameObject> _talentSlots = new List<GameObject>();
    #endregion

    #region 내부 변수
    // 슬롯 해금 상태 관리 배열
    private bool[] _unlockedStates;
    #endregion

    private void Start()
    {
        _unlockedStates = new bool[_talentSlots.Count];

        InitSlots();
    }

    #region 초기화
    // 슬롯 초기 상태 설정
    private void InitSlots()
    {
        for (int i = 0; i < _talentSlots.Count; i++)
        {
            Transform slotTr = _talentSlots[i].transform;

            GameObject lockObj = slotTr.Find("Lock").gameObject;
            GameObject openObj = slotTr.Find("Open").gameObject;
            GameObject checkObj = slotTr.Find("Check").gameObject;

            if (_unlockedStates[i])
            {
                lockObj.SetActive(false);
                openObj.SetActive(true);
            }
            else
            {
                lockObj.SetActive(true);
                openObj.SetActive(false);
            }

            checkObj.SetActive(false);
        }

        // 가운데 슬롯 기본 해금
        if (!_unlockedStates[4])
        {
            _unlockedStates[4] = true;

            Transform centerTr = _talentSlots[4].transform;
            centerTr.Find("Lock").gameObject.SetActive(false);
            centerTr.Find("Open").gameObject.SetActive(true);

            _popups[4].Unlock();
        }
    }
    #endregion

    #region 외부 호출 함수
    // 랜덤 슬롯 해금 요청
    public void UnlockRandomSlot()
    {
        List<int> lockedIndices = new List<int>();

        for (int i = 0; i < _talentSlots.Count; i++)
        {
            if (!_unlockedStates[i])
                lockedIndices.Add(i);
        }

        if (lockedIndices.Count == 0)
            return;

        // 마지막 1개 처리
        if (lockedIndices.Count == 1)
        {
            StartCoroutine(CoOpenLast(lockedIndices[0]));
            return;
        }

        // 랜덤 연출 시작
        StartCoroutine(CoRandomHighlightEffect());
    }
    #endregion

    #region 코루틴
    // 마지막 1개 해금 처리
    private IEnumerator CoOpenLast(int index)
    {
        Transform highlightTr = _talentSlots[index].transform.Find("Highlight");

        if (highlightTr != null)
            highlightTr.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        if (highlightTr != null)
            highlightTr.gameObject.SetActive(false);

        OpenSlot(index);
    }

    // 룰렛 하이라이트 연출
    private IEnumerator CoRandomHighlightEffect()
    {
        List<int> lockedIndices = new List<int>();

        for (int i = 0; i < _talentSlots.Count; i++)
        {
            if (!_unlockedStates[i])
                lockedIndices.Add(i);
        }

        if (lockedIndices.Count == 0)
            yield break;

        int finalIndex = lockedIndices[Random.Range(0, lockedIndices.Count)];

        float delay = 0.09f;
        int prevIndex = -1;

        for (int i = 0; i < 10; i++)
        {
            int randomIndex = lockedIndices[Random.Range(0, lockedIndices.Count)];

            SetAllHighlightOff();

            Transform highlightTr = _talentSlots[randomIndex].transform.Find("Highlight");

            if (highlightTr != null)
            {
                highlightTr.gameObject.SetActive(true);

                // 슬롯 변경 시 사운드
                if (randomIndex != prevIndex)
                {
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.SFXPlay(ESfxType.Roullet);

                    prevIndex = randomIndex;
                }
            }

            yield return new WaitForSeconds(delay);
        }

        // 최종 선택
        SetAllHighlightOff();

        Transform finalTr = _talentSlots[finalIndex].transform.Find("Highlight");

        if (finalTr != null)
            finalTr.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        SetAllHighlightOff();

        OpenSlot(finalIndex);
    }
    #endregion

    #region 내부 함수
    // 슬롯 해금 처리
    private void OpenSlot(int index)
    {
        Debug.Log("열린 index: " + index);

        GameObject slot = _talentSlots[index];
        Transform slotTr = slot.transform;

        slotTr.Find("Lock").gameObject.SetActive(false);
        slotTr.Find("Open").gameObject.SetActive(true);

        _unlockedStates[index] = true;

        _popups[index].Unlock();

        CPlayerDataManager.Instance.AddTalentLevel(index);
    }

    // 모든 하이라이트 비활성화
    private void SetAllHighlightOff()
    {
        for (int i = 0; i < _talentSlots.Count; i++)
        {
            Transform highlightTr = _talentSlots[i].transform.Find("Highlight");

            if (highlightTr != null)
                highlightTr.gameObject.SetActive(false);
        }
    }
    #endregion

    #region 선택 처리
    // 슬롯 선택 처리
    public void SelectSlot(GameObject selectedSlot)
    {
        // 기존 체크 제거
        SetAllCheckOff();

        // 선택된 슬롯 체크 연출
        Transform checkTr = selectedSlot.transform.Find("Check");

        if (checkTr != null)
        {
            StartCoroutine(CoCheckEffect(selectedSlot.transform));
        }
    }
    // 모든 체크 비활성화
    private void SetAllCheckOff()
    {
        for (int i = 0; i < _talentSlots.Count; i++)
        {
            Transform checkTr = _talentSlots[i].transform.Find("Check");

            if (checkTr != null)
                checkTr.gameObject.SetActive(false);
        }
    }

    // 체크 표시 연출
    private IEnumerator CoCheckEffect(Transform slotTr)
    {
        Transform checkTr = slotTr.Find("Check");

        if (checkTr != null)
        {
            checkTr.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            checkTr.gameObject.SetActive(false);
        }
    }
    #endregion
}