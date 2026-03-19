using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerState

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 매 프레임 정보를 받아 플레이어 상태 결정
*/

[RequireComponent(typeof(InputState))]
public class PlayerState : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Moving,
        Dead,
    }

    #region 내부 변수
    public event System.Action<EState> OnPlayerStateChanged;
    private EState _state = EState.Idle;
    private InputState _inputState;
    #endregion

    private void Awake()
    {
        _inputState = GetComponent<InputState>();

        if (_inputState == null)
        {
            Debug.LogError("PlayerState _inputState 참조 실패");
            return;
        }
    }

    void Update()
    {
        SetPlayerState(DecidePlayerState());
    }

    // 단일 상태 진입점
    private void SetPlayerState(EState next)
    {
        if (_state == next)
        {
            return;
        }

        _state = next;

        switch (_state)
        {
            case EState.Idle:

                break;

            case EState.Moving:

                break;

            case EState.Dead:

                break;
        }

        OnPlayerStateChanged?.Invoke(_state);
    }

    // 상태 결정
    private EState DecidePlayerState()
    {
        //if () Status 에서 HP = 0 일경우 IsDead를 받아와서 조건문에 넣으면 될 듯???
        //{
        //    return EState.Dead;
        //}

        if (_inputState.GetInputState() != InputState.EState.Idle) // 플레이어무버 쪽에서 isMoving 조건문 추가해야함
        {
            return EState.Moving;
        }

        return EState.Idle;
    }

    #region 외부 호출 함수
    public EState GetPlayerState()
    {
        return _state;
    }
    #endregion
}