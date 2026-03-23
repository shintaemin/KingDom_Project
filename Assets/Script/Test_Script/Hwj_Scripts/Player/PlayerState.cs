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
    public event System.Action<EState> OnStateChanged;
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
        SetState(DecideState());
    }

    // 단일 상태 진입점
    private void SetState(EState next)
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

        OnStateChanged?.Invoke(_state);
    }

    // 상태 결정
    private EState DecideState()
    {
        //if () Status 에서 HP = 0 일경우 IsDead를 받아와서 조건문에 넣으면 될 듯???
        //{
        //    return EState.Dead;
        //}

        if (_inputState.GetState() != InputState.EState.Idle) // 플레이어무버 쪽에서 isMoving 조건문 추가해야함
        {
            return EState.Moving;
        }

        return EState.Idle;
    }

    #region 외부 호출 함수
    public EState GetState()
    {
        return _state;
    }
    #endregion
}