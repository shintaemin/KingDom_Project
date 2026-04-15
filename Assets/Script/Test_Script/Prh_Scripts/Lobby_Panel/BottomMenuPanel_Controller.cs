using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 하단메뉴 패널 전환 관리
/*
 ▶ 할일
  - 하단 메뉴 버튼 입력에 따라 해당 패널을 활성화
  - 활성화된 패널을 제외한 나머지 패널은 모두 비활성화
  - 초기 진입 시 기본 패널(Lobby) 표시

 ※ 참고사항
  - 모든 패널은 GameObject 활성/비활성으로 제어
  - 패널 전환 시 항상 HideAllPanels() 호출 후 활성화

  - 박라희
*/
#endregion

public class BottomMenuPanel_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("패널 목록")]
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _equipmentShopPanel;
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private GameObject _talentPanel;
    #endregion

    private void Start()
    {
        // 기본 패널 : Lobby
        ShowLobbyPanel();
    }

    #region 내부 함수
    // 모든 패널 비활성화
    private void HideAllPanels()
    {
        if (_shopPanel != null) _shopPanel.SetActive(false);
        if (_equipmentShopPanel != null) _equipmentShopPanel.SetActive(false);
        if (_lobbyPanel != null) _lobbyPanel.SetActive(false);
        if (_upgradePanel != null) _upgradePanel.SetActive(false);
        if (_talentPanel != null) _talentPanel.SetActive(false);
    }
    #endregion

    #region 외부 호출 함수
    // 상점 활성화
    public void ShowShopPanel()
    {
        HideAllPanels();
        _shopPanel.SetActive(true);
    }

    // 장비상점 활성화
    public void ShowEquipmentShopPanel()
    {
        HideAllPanels();
        _equipmentShopPanel.SetActive(true);
    }

    // 로비 활성화
    public void ShowLobbyPanel()
    {
        HideAllPanels();
        _lobbyPanel.SetActive(true);
    }

    // 업데이트 활성화
    public void ShowUpgradePanel()
    {
        HideAllPanels();
        _upgradePanel.SetActive(true);
    }

    // 재능 활성화
    public void ShowTalentPanel()
    {
        HideAllPanels();
        _talentPanel.SetActive(true);
    }
    #endregion
}
