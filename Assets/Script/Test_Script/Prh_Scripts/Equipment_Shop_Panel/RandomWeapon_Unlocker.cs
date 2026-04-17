using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 아이템 랜덤 해금 관리
/*
 ▶ 할일
  - 잠긴 무기 슬롯 중 하나를 랜덤으로 선택하여 해금
  - 슬롯 하이라이트를 순차적으로 이동시키며 연출
  - 최종 선택된 슬롯을 해금하고 선택 상태로 반영
  - 보상 팝업 및 사운드 출력

 ▶ 흐름
  1. 모든 슬롯 검사 후 잠긴 슬롯만 수집
  2. 랜덤 타겟 슬롯 선정
  3. 하이라이트를 순차 이동 (점점 느려짐)
  4. 최종 슬롯 도달 후 해금 및 선택 처리

 ※ 참고사항
  - 이미 해금된 슬롯은 제외하고 진행
  - 마지막 1개 남은 경우 즉시 해금 처리
  - 연출 속도는 startDelay ~ endDelay로 점진적으로 증가
  - WeaponSelect_Controller를 통해 선택 상태 동기화

- 박라희
*/
#endregion

public class RandomWeapon_Unlocker : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private List<GameObject> _weaponSlots;
    [SerializeField] private GameObject _rewardPopup;

    [SerializeField] private float _startDelay = 0.05f;
    [SerializeField] private float _endDelay = 0.3f;

    [SerializeField] private TMPro.TextMeshProUGUI _priceText;

    [SerializeField] private GameObject _openButton;
    [SerializeField] private GameObject _lockButton;
    #endregion

    #region 내부 변수
    // 연출 진행 여부
    private bool _isRolling = false;
    #endregion

    void Start()
    {
        UpdatePrice();
    }

    private void OnEnable()
    {
        UpdatePrice();
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        int price = Mathf.Max(1000, CPlayerDataManager.Instance.UnLockedWeaponCount * 1000);
        int gem = CPlayerDataManager.Instance.Gem;

        bool canBuy = gem >= price;

        _openButton.SetActive(canBuy);
        _lockButton.SetActive(!canBuy);
    }

    #region 외부 호출 함수
    // 랜덤 해금 시작 요청
    public void StartRandomUnlock()
    {
        if (_isRolling)
            return;

        // 가격 계산
        int price = CPlayerDataManager.Instance.UnLockedWeaponCount * 1000;

        // 다이아 체크
        if (!CPlayerDataManager.Instance.TryUseGem(price))
        {
            Debug.Log("다이아 부족");
            return;
        }

        StartCoroutine(CoRandomUnlock());
    }
    #endregion

    void UpdatePrice()
    {
        int price = Mathf.Max(1000, CPlayerDataManager.Instance.UnLockedWeaponCount * 1000);
        _priceText.text = price.ToString();

        UpdateButtonState(); //다이아 버튼
    }

    #region 내부 코루틴
    // 랜덤 해금 연출 처리
    private IEnumerator CoRandomUnlock()
    {
        _isRolling = true;

        List<int> lockedIndices = new List<int>();

        // 잠긴 슬롯 인덱스 수집
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            GameObject slot = _weaponSlots[i];
            if (slot == null) continue;

            Transform lockTr = slot.transform.Find("Lock");

            if (lockTr != null && lockTr.gameObject.activeSelf)
                lockedIndices.Add(i);
        }

        // 해금 가능한 슬롯 없으면 종료
        if (lockedIndices.Count == 0)
        {
            _isRolling = false;
            yield break;
        }

        int targetIndex = lockedIndices[Random.Range(0, lockedIndices.Count)];

        // 마지막 1개만 남은 경우
        if (lockedIndices.Count == 1)
        {
            int lastIndex = lockedIndices[0];

            SetHighlight(lastIndex);
            yield return new WaitForSeconds(0.1f);

            GameObject selectedSlot = _weaponSlots[lastIndex];

            SetUnlock(selectedSlot);
            SelectByController(selectedSlot);

            ShowReward();

            SetAllHighlightOff();
            _isRolling = false;
            yield break;
        }

        // 랜덤 순회 횟수
        int loopCount = Random.Range(6, 10);

        for (int i = 0; i < loopCount; i++)
        {
            int randomPos = Random.Range(0, lockedIndices.Count);
            int slotIndex = lockedIndices[randomPos];

            SetHighlight(slotIndex);

            // 점점 느려지는 연출
            float t = (float)i / loopCount;
            t = t * t;

            float delay = Mathf.Lerp(_startDelay, _endDelay, t);
            yield return new WaitForSeconds(delay);
        }

        // 최종 선택
        SetHighlight(targetIndex);
        yield return new WaitForSeconds(_endDelay * 1.5f);

        GameObject finalSlot = _weaponSlots[targetIndex];

        UpdatePrice();


        SetUnlock(finalSlot);
        SelectByController(finalSlot);

        ShowReward();

        SetAllHighlightOff();
        _isRolling = false;
    }
    #endregion

    #region 내부 함수
    // 컨트롤러를 통해 슬롯 선택 처리
    private void SelectByController(GameObject slot)
    {
        var controller = FindObjectOfType<WeaponSelect_Controller>();

        if (controller != null)
        {
            controller.SelectSlot(slot);
        }
    }

    // 보상 UI 및 사운드 출력
    private void ShowReward()
    {
        if (_rewardPopup != null)
            _rewardPopup.SetActive(true);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SFXPlay(ESfxType.Item_Unlock);
    }

    // 슬롯 해금 처리
    private void SetUnlock(GameObject slot)
    {
        Transform lockTr = slot.transform.Find("Lock");
        Transform openTr = slot.transform.Find("Open");

        if (lockTr != null) lockTr.gameObject.SetActive(false);
        if (openTr != null) openTr.gameObject.SetActive(true);
    }

    // 특정 슬롯 하이라이트 표시
    private void SetHighlight(int index)
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            Transform highlightTr = _weaponSlots[i].transform.Find("Highlight");

            if (highlightTr != null)
            {
                bool isOn = (i == index);

                // 새로 켜질 때 사운드 재생
                if (isOn && !highlightTr.gameObject.activeSelf)
                {
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.SFXPlay(ESfxType.Roullet);
                }

                highlightTr.gameObject.SetActive(isOn);
            }
        }
    }

    // 모든 하이라이트 비활성화
    private void SetAllHighlightOff()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            Transform highlightTr = _weaponSlots[i].transform.Find("Highlight");

            if (highlightTr != null)
                highlightTr.gameObject.SetActive(false);
        }
    }
    #endregion
}
