using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 보상 팝업 UI 제어
/*
 ▶ 할일
  - 보상 수령 버튼 클릭 시 팝업을 닫음

 ▶ 흐름
  1. 버튼 클릭 시 호출
  2. 팝업 오브젝트 존재 여부 확인
  3. 팝업 비활성화

 ※ 참고사항
  - 버튼 OnClick 이벤트와 연결하여 사용

  - 박라희
*/
#endregion

public class RewardPopup_Controller : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _popup;
    #endregion

    #region 외부 호출 함수
    // 보상 수령 버튼 클릭 처리
    public void OnClickReceive()
    {
        // 팝업이 존재하면 닫기
        if (_popup != null)
        {
            _popup.SetActive(false);
        }
    }
    #endregion
}
