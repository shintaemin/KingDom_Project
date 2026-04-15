using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 의상 슬롯 버튼 입력 처리
/*
 ▶ 할일
  - 슬롯 클릭 시 ClothesSelect_Controller에 선택 요청 전달
  - 현재 슬롯(GameObject)을 컨트롤러에 전달하여 선택 처리

 ※ 참고사항
  - 버튼 OnClick 이벤트와 연결하여 사용
  - 컨트롤러가 존재할 경우에만 동작

  - 박라희
*/
#endregion

public class ClothesSlot_Button : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private ClothesSelect_Controller _controller;
    #endregion

    #region 외부 호출 함수
    // 슬롯 클릭 처리
    public void OnClickSlot()
    {
        // 컨트롤러가 없으면 실행 중단
        if (_controller == null)
            return;

        // 현재 슬롯 선택 요청
        _controller.SelectSlot(gameObject);
    }
    #endregion
}
