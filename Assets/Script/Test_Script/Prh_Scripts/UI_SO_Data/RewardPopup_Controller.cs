using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image _popUpImage;
    [SerializeField] private Image _statImage;
    [SerializeField] private TextMeshProUGUI _statText;
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

    public void SetPopup(Sprite image, Sprite option, string text)
    {
        _popUpImage.sprite = image;
        _statImage.sprite = option;
        _statText.text = text;
        _statText.fontSize = 60;
        _statText.fontStyle = FontStyles.Bold;
        _statText.color = Color.white;
    }
    #endregion
}
