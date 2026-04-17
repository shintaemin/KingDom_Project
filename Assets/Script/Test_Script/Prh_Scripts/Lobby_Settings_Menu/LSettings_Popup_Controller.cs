using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 설정 메뉴 UI 상태 제어
/*
 ▶ 할일
  - 하단 메뉴(4버튼) 토글 제어
  - 설정 버튼 클릭 시 팝업 열기 및 메뉴 숨김
  - 팝업 닫기 처리

 ▶ 흐름
  1. 메뉴 버튼 클릭 → 메뉴바 토글 (On / Off)
  2. 설정 버튼 클릭 → 메뉴바 숨기고 팝업 표시
  3. 닫기 버튼 클릭 → 팝업 비활성화

  - 박라희
*/
#endregion

public class LSettings_Popup_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("Panels")]
    [SerializeField] private GameObject _menu4Bar;
    [SerializeField] private GameObject _menuPopup;
    #endregion

    #region 외부 호출 함수
    // 메뉴 버튼 클릭 → 메뉴바 토글
    public void OpenMenuBar()
    {
        if (_menu4Bar == null)
            return;

        // 현재 상태 반전 (On ↔ Off)
        _menu4Bar.SetActive(!_menu4Bar.activeSelf);
    }

    // 설정 버튼 클릭 → 팝업 열기 + 메뉴 숨김
    public void OpenPopup()
    {
        if (_menuPopup != null)
            _menuPopup.SetActive(true);

        // 팝업 열 때 메뉴는 비활성화
        if (_menu4Bar != null)
            _menu4Bar.SetActive(false);
    }

    // 닫기 버튼 클릭 → 팝업 닫기
    public void ClosePopup()
    {
        if (_menuPopup == null)
            return;

        _menuPopup.SetActive(false);
    }
    #endregion
}
