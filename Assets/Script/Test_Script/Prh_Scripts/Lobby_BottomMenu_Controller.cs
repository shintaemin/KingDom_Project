using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 하단 메뉴 버튼 관리
/*
 ▶ 할일
  - 하단 메뉴 버튼의 선택 상태 관리
  - 플레이어 레벨에 따라 버튼 잠금 상태 갱신

 ※ 동작 흐름
  1. 시작 시 플레이어 레벨 기준으로 버튼 잠금 상태 갱신
  2. 기본 선택 인덱스가 유효하면 해당 버튼 선택
  3. 불가능하면 선택 가능한 첫 번째 버튼 선택
*/
#endregion

public class Lobby_BottomMenu_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("버튼 목록")]
    [SerializeField] private Lobby_TabButton_Visual[] _buttons;
    [Header("플레이어 레벨")]
    [SerializeField] private int _playerLevel = 1;
    [Header("기본 선택 인덱스")]
    // 로비가 기본
    [SerializeField] private int _defaultSelectedIndex = 2;
    #endregion

    #region 내부 변수
    // 현재 선택된 버튼 번호
    private int _currentSelectedIndex;
    #endregion

    
    private void Start()
    {
        // 기본 선택 인덱스 설정
        _currentSelectedIndex = _defaultSelectedIndex;

        // 버튼 잠금 상태 갱신, 레벨5면 장비장점, 레벨10이면 재능 열림
        RefreshUnlockStates();

        // 기본 선택이 가능하면 선택
        if (IsValidIndex(_currentSelectedIndex) && !_buttons[_currentSelectedIndex].IsLocked)
        {
            SelectButton(_currentSelectedIndex);
            return;
        }

        // 아니면 선택 가능한 첫 버튼 선택
        SelectFirstUnlockedButton();
    }

    #region 내부 함수
    // 모든 버튼의 잠금 상태를 현재 레벨 기준으로 갱신
    private void RefreshUnlockStates()
    {
        if (_buttons == null)
        {
            return;
        }

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i] == null)
            {
                continue;
            }
            // 잠겨야 하는지 판단
            _buttons[i].UpdateLockState(_playerLevel);
        }
    }

    // 선택 가능한 첫 번째 버튼을 찾아 선택
    private void SelectFirstUnlockedButton()
    {
        if (_buttons == null)
        {
            return;
        }

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i] == null)
            {
                continue;
            }

            if (_buttons[i].IsLocked)
            {
                continue;
            }

            SelectButton(i);
            return;
        }
    }

    // 버튼 인덱스가 유효한지 검사
    private bool IsValidIndex(int index)
    {
        if (_buttons == null || _buttons.Length == 0)
        {
            return false;
        }

        if (index < 0 || index >= _buttons.Length)
        {
            return false;
        }

        if (_buttons[index] == null)
        {
            return false;
        }

        return true;
    }
    #endregion

    #region 외부 호출 함수
    // 버튼 선택 처리
    public void SelectButton(int index)
    {
        // 잘못된 인덱스 방지
        if (!IsValidIndex(index))
        {
            return;
        }

        // 잠긴 버튼은 선택 불가
        if (_buttons[index].IsLocked)
        {
            return;
        }
        _currentSelectedIndex = index;

        // 선택된 버튼만 활성화
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i] == null)
            {
                continue;
            }

            _buttons[i].SetSelected(i == _currentSelectedIndex);
        }
    }

    // 플레이어 레벨 변경 및 버튼 상태 갱신
    public void SetPlayerLevel(int newLevel)
    {
        _playerLevel = newLevel;

        // 잠금 상태 갱신
        RefreshUnlockStates();

        // 현재 선택이 불가능하면 재선택
        if (!IsValidIndex(_currentSelectedIndex) || _buttons[_currentSelectedIndex].IsLocked)
        {
            SelectFirstUnlockedButton();
            return;
        }

        // 가능하면 기존 선택 유지
        SelectButton(_currentSelectedIndex);
    }

    // 플레이어 레벨 증가
    public void AddLevel(int amount)
    {
        _playerLevel += amount;
        SetPlayerLevel(_playerLevel);
    }
    #endregion
}
