using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 업그레이드 버튼 처리
/*
 ▶ 할일
  - 버튼 클릭 시 다이아를 소비하여 업그레이드 시도
  - 다이아가 부족하면 업그레이드 진행 차단

 ※ 구조 의도
  - UI 입력(버튼)과 실제 업그레이드 로직을 분리
  - 다이아 체크는 TopBar(UI)에서 담당하고, 결과만 전달받음

 ▶ 흐름
  1. 버튼 클릭 → OnClickUpgrade 호출
  2. LTopBar_UI에서 다이아 사용 시도
  3. 실패 시 업그레이드 중단
  4. 성공 시 이후 업그레이드 로직 실행 가능

 ※ 참고사항
  - cost 값은 인스펙터에서 설정
  - 현재는 로그만 출력, 실제 업그레이드 로직은 외부에서 연결 필요

  - 박라희
*/
#endregion

public class Upgrade_Button : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _openButton;
    [SerializeField] private GameObject _lockButton;

    [SerializeField] private TMP_Text _priceText;
    #endregion
    void UpdateButtonState()
    {
        var player = CPlayerDataManager.Instance;

        int price = Mathf.Max(1000, player.CurrentTalentSum * 1000);
        int gem = player.Gem;

        _priceText.text = price.ToString();

        bool canBuy = gem >= price;

        _openButton.SetActive(canBuy);
        _lockButton.SetActive(!canBuy);
    }

    void OnEnable()
    {
        CPlayerDataManager.Instance.OnStatChanged += UpdateButtonState;
        UpdateButtonState();
    }

    void OnDisable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= UpdateButtonState;
    }

    #region 외부 호출 함수
    // 업그레이드 버튼 클릭 처리
    public void OnClickUpgrade()
    {
        var player = CPlayerDataManager.Instance;

        int price = Mathf.Max(1000, player.CurrentTalentSum * 1000);

        // 다이아 사용 시도
        bool success = player.TryUseGem(price);

        // 다이아 부족 시 중단
        if (!success)
        {
            Debug.Log("다이아 부족");
            return;
        }

        // 성공 시 (이후 업그레이드 로직 연결 지점)
        Debug.Log("구매 성공");
    }
    #endregion
}
