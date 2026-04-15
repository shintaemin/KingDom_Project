using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 재능 해금 팝업 제어
/*
 ▶ 할일
  - 재능 해금 시 팝업 UI를 활성화
  - 동일 슬롯에서 팝업이 중복 실행되지 않도록 제어

 ※ 참고사항
  - 팝업패널은 개별 슬롯의 팝업 UI
  - 부모 오브젝트까지 함께 활성화하여 UI 전체 표시
  - 한 번 표시된 팝업은 다시 실행되지 않도록 상태 관리

 ※ 동작 흐름
  1. Unlock() 호출
  2. 이미 실행된 팝업인지 검사
  3. 부모 오브젝트 활성화
  4. 팝업 패널 활성화

  - 박라희
*/
#endregion

public class Talent_Slot_Popup : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _popupPanel;
    #endregion

    #region 내부 변수
    // 팝업 중복 실행 방지
    private bool _isPopupShown = false;
    #endregion

    #region 외부 호출 함수
    // 재능 해금 시 팝업 표시
    public void Unlock()
    {
        // 이미 팝업이 표시된 경우 실행 중단
        if (_isPopupShown)
            return;

        // 부모 오브젝트
        GameObject parent = _popupPanel.transform.parent.gameObject;

        // 부모 활성화 → UI 전체 보이기
        parent.SetActive(true);

        // 개별 팝업 활성화
        _popupPanel.SetActive(true);

        // 재실행 방지 상태 설정
        _isPopupShown = true;
    }
    #endregion
}
