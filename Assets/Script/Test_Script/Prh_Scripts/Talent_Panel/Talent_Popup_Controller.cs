using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 재능 팝업 UI 제어
/*
 ▶ 할일
  - 재능 팝업 UI의 활성/비활성 제어
  - 닫기 버튼 클릭 시 팝업 비활성화

 ※ 참고사항
  - 버튼 OnClick 이벤트와 연결하여 사용

  - 박라희
*/
#endregion

public class Talent_Popup_Controller : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _popup;
    #endregion

    #region 외부 호출 함수
    // 팝업 닫기 처리
    public void ClosePopup()
    {
        // 팝업 비활성화
        if (_popup != null)
            _popup.SetActive(false);
    }
    #endregion
}
