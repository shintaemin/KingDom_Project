using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region 토글 스위치 UI 제어
/*
 ▶ 할일
  - On / Off 상태에 따라 스위치 위치 및 색상 변경
  - 클릭 시 상태를 반전시키고 UI에 즉시 반영

 ▶ 흐름
  1. Start에서 초기 상태 적용
  2. Toggle() 호출 시 상태 반전
  3. ApplyState()에서 위치 + 색상 UI 반영

 ※ 참고사항
  - handle : 스위치 버튼 (움직이는 요소)
  - anchoredPosition으로 좌 / 우 이동 처리
  - 색상 변경으로 On 상태를 시각적으로 강조
*/
#endregion

public class LSettings_Toggle_Switch : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private RectTransform _handle;
    [SerializeField] private Image _handleImage;

    [Header("위치")]
    [SerializeField] private Vector2 _onPos;
    [SerializeField] private Vector2 _offPos;

    [Header("색상")]
    [SerializeField] private Color _onColor = Color.green;
    [SerializeField] private Color _offColor = Color.gray;

    [Header("Icon")]
    [SerializeField] public GameObject _onIcon;
    [SerializeField] public GameObject _offIcon;
    #endregion

    #region 내부 변수
    // 현재 토글 상태
    private bool _isOn = false;

    #endregion

    private void Awake()
    {
        // 초기 상태 UI 반영
        ApplyState();
    }

    #region 외부 호출 함수

    // 토글 버튼 클릭 시 호출
    public void Toggle()
    {
        // 상태 반전
        _isOn = !_isOn;

        // UI 갱신
        ApplyState();
    }

    public void SetOn(bool value)
    {
        _isOn = value;
        ApplyState();
    }
    #endregion

    #region 내부 함수

    // 현재 상태를 UI에 반영
    private void ApplyState()
    {
        // 위치 변경 (좌 / 우 이동)
        _handle.anchoredPosition = _isOn ? _onPos : _offPos;

        // 색상 변경 (On 강조 / Off 기본)
        _handleImage.color = _isOn ? _onColor : _offColor;

        // 아이콘 변경
        if (_onIcon != null) _onIcon.SetActive(_isOn);
        if (_offIcon != null) _offIcon.SetActive(!_isOn);
    }

    #endregion
}
