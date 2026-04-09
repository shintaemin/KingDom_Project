using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region 탭 버튼 UI 상태 제어
/*
 ▶ 할일
  - 버튼의 상태(일반 / 선택 / 잠금)에 따라 UI 변경
  - 플레이어 레벨에 따라 잠금 여부 결정

 ※ 상태 우선순위
  - 잠금 상태 > 선택 상태 > 일반 상태
*/
#endregion

public class Lobby_TabButton_Visual : MonoBehaviour
{
    #region 인스펙터
    [Header("참조")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private RectTransform _iconRect;
    [SerializeField] private GameObject _iconObject;
    [SerializeField] private GameObject _textObject;
    [SerializeField] private GameObject _lockObject;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private Button _button;

    [Header("스프라이트")]
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _lockedSprite;

    [Header("해금 레벨")]
    [SerializeField] private int _unlockLevel = 1;

    [Header("사이즈")]
    [SerializeField] private float _normalWidth = 100f;
    [SerializeField] private float _selectedWidth = 160f;
    [SerializeField] private float _height = 90f;

    [Header("아이콘 스케일")]
    [SerializeField] private Vector3 _normalIconScale = Vector3.one;
    [SerializeField] private Vector3 _selectedIconScale = new Vector3(1.15f, 1.15f, 1f);
    #endregion

    #region 내부 변수
    private bool _isLocked = false;
    #endregion

    #region 프로퍼티
    public int UnlockLevel => _unlockLevel;
    public bool IsLocked => _isLocked;
    #endregion
    
    // 버튼 레이아웃 재사용
    private void Reset()
    {
        // 자동 컴포넌트 캐싱
        _button = GetComponent<Button>();
        _layoutElement = GetComponent<LayoutElement>();
    }

    #region 내부 함수
    // 일반 상태 적용
    private void ApplyNormal()
    {
        if (_backgroundImage != null)
        {
            // 일반 상태에서는 배경 숨김
            _backgroundImage.gameObject.SetActive(false);
            if (_normalSprite != null)
                // 일반 스프라이트 지정
                _backgroundImage.sprite = _normalSprite;
        }

        if (_iconObject != null)
            // 아이콘 켜기
            _iconObject.SetActive(true);

        if (_iconRect != null)
            // 아이콘 원래 크기로
            _iconRect.localScale = _normalIconScale;

        if (_textObject != null)
            // 일반 상태에서는 텍스트 안보이게 
            _textObject.SetActive(false);

        if (_layoutElement != null)
        {
            // 버튼 크기를 일반 상태 크기로 바꿈
            _layoutElement.preferredWidth = _normalWidth;
            _layoutElement.preferredHeight = _height;
        }
    }

    // 선택 상태 적용
    private void ApplySelected()
    {
        if (_backgroundImage != null)
        {
            // 선택되면 배경 켜기
            _backgroundImage.gameObject.SetActive(true);
            if (_selectedSprite != null)
                // 선택된 이미지로 변경
                _backgroundImage.sprite = _selectedSprite;
        }

        if (_iconObject != null)
            // 아이콘 계속 켜기
            _iconObject.SetActive(true);

        if (_iconRect != null)
            // 선택되면 아이콘 켜기
            _iconRect.localScale = _selectedIconScale;

        if (_textObject != null)
            // 선택되면 텍스트 켜기
            _textObject.SetActive(true);

        if (_layoutElement != null)
        {
            // 선택된 버튼 가로폭 
            _layoutElement.preferredWidth = _selectedWidth;
            // 선택된 버튼 높이
            _layoutElement.preferredHeight = _height;
        }
    }

    // 잠금 상태 적용
    private void ApplyLocked()
    {
        if (_backgroundImage != null)
            // 잠기면 배경 끄기
            _backgroundImage.gameObject.SetActive(false);

        if (_iconObject != null)
            _iconObject.SetActive(false);

        if (_textObject != null)
            _textObject.SetActive(false);

        if (_iconRect != null)
            // 아이콘 원래 크기로 돌아감
            _iconRect.localScale = _normalIconScale;

        if (_layoutElement != null)
        {
            // 장금 상태에서 일반 크기로 돌아감
            _layoutElement.preferredWidth = _normalWidth;
            _layoutElement.preferredHeight = _height;
        }
    }
    #endregion

    #region 외부 호출 함수
    // 레벨 기준 잠금 상태 갱신
    public void UpdateLockState(int playerLevel)
    {
        bool shouldLock = playerLevel < _unlockLevel;
        SetLocked(shouldLock);
    }

    // 잠금 상태 설정
    public void SetLocked(bool locked)
    {
        _isLocked = locked;

        if (_lockObject != null)
            // 자물쇠 잠기면 켜짐, 안잠기면 꺼짐
            _lockObject.SetActive(locked);

        if (_button != null)
            // 잠겼을때 버튼 클릭 막기
            _button.interactable = !locked;

        if (locked)
        {
            // 잠기면 자물쇠, 아니면 일반 적용
            ApplyLocked();
            return;
        }

        ApplyNormal();
    }

    // 선택 상태 설정
    public void SetSelected(bool selected)
    {
        // 잠금 상태가 우선, 선택 무시
        if (_isLocked)
        {
            ApplyLocked();
            return;
        }

        if (selected)
        {
            // 선택 됐으면 선택 이미지
            ApplySelected();
            return;
        }
        // 선택 아니면 일반 상태
        ApplyNormal();
    }
    #endregion
}
