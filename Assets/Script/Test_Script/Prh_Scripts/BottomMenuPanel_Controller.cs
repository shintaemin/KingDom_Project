using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 하단메뉴 패널 전환 관리
/*
 ▶ 할일
  - 하단 메뉴 버튼 클릭 시 해당 패널을 활성화
  - 나머지 패널은 모두 비활성화
*/
#endregion

public class BottomMenuPanel_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("패널 목록")]
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _equipmentShopPanel;
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _updatePanel;
    [SerializeField] private GameObject _talentPanel;
    #endregion

    private void Start()
    {
        // 기본 패널 : Lobby
        ShowLobbyPanel();
    }

    // 모든 패널 비활성화
    private void HideAllPanels()
    {
        if (_shopPanel != null) _shopPanel.SetActive(false);
        if (_equipmentShopPanel != null) _equipmentShopPanel.SetActive(false);
        if (_lobbyPanel != null) _lobbyPanel.SetActive(false);
        if (_updatePanel != null) _updatePanel.SetActive(false);
        if (_talentPanel != null) _talentPanel.SetActive(false);
    }

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
    public void ShowUpdatePanel()
    {
        HideAllPanels();
        _updatePanel.SetActive(true);
    }

    // 재능 활성화
    public void ShowTalentPanel()
    {
        HideAllPanels();
        _talentPanel.SetActive(true);
    }
}
