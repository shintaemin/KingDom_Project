using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 언어 설정 팝업 제어
/*
 ▶ 할일
  - 언어 설정 팝업 UI의 활성 / 비활성 제어
  - 버튼 클릭 시 팝업 열기 / 닫기 처리

 ▶ 흐름
  1. OpenPopup() 호출 → 팝업 활성화
  2. ClosePopup() 호출 → 팝업 비활성화

 ※ 참고사항
  - 버튼 OnClick 이벤트와 연결하여 사용
  - targetPopup은 실제 표시될 UI 오브젝트

  - 박라희
*/
#endregion

public class LSettings_language_PopUp : MonoBehaviour
{
    #region 인스펙터
    public GameObject targetPopup;
    #endregion

    #region 외부 호출 함수
    public void OpenPopup()
    {
        targetPopup.SetActive(true);
    }

    public void ClosePopup()
    {
        targetPopup.SetActive(false);
    }
    #endregion
}
