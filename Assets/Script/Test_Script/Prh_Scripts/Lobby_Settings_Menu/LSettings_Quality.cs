using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 그래픽 옵션 UI 선택 상태 관리
/*
 ▶ 할일
  - Low / High 버튼 선택 상태를 UI로 표시
  - 선택된 항목에 On 아이콘 활성화

  - 박라희
*/
#endregion

public class LSettings_Quality : MonoBehaviour
{
    #region 인스펙터
    [Header("Low")]
    [SerializeField] private GameObject _lowOnIcon;
    [Header("High")]
    [SerializeField] private GameObject _highOnIcon;
    #endregion

    #region 내부 변수
    private bool _isHigh = false;
    #endregion

    private void Awake()
    {
        // 초기 UI 상태 적용
        ApplyState();
    }

    #region 외부 호출 함수
    // Low
    public void SelectLow()
    {
        _isHigh = false;
        ApplyState();
    }

    // High
    public void SelectHigh()
    {
        _isHigh = true;
        ApplyState();
    }
    #endregion

    #region 내부 함수
    // 선택 상태에 따른 UI 반영
    private void ApplyState()
    {
        if (_lowOnIcon != null) _lowOnIcon.SetActive(!_isHigh);
        if (_highOnIcon != null) _highOnIcon.SetActive(_isHigh);
    }
    #endregion
}
