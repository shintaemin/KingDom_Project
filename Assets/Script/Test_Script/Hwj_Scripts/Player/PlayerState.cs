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
    public event System.Action OnDead;
    private EState _state = EState.Idle;
    private InputState _inputState;
    private HpSystem _hpSystem;

    [HideInInspector] public bool IsMoving = false;
    #endregion

    private void Awake()
    {
        _inputState = GetComponent<InputState>();

        if (_inputState == null)
        {
            Debug.LogError("PlayerState _inputState 참조 실패");
            return;
        }

        _hpSystem = GetComponent<HpSystem>();

        if (_hpSystem == null)
        {
            Debug.LogError("PlayerState _hpSystem 참조 실패");
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
        if (_state == EState.Dead)
        {
            return;
        }

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
                OnDead?.Invoke();
                break;
        }

        OnStateChanged?.Invoke(_state);
    }

    // 상태 결정
    private EState DecideState()
    {
        if (_hpSystem.IsDead)
        {
            return EState.Dead;
        }

        if (_inputState.GetState() != InputState.EState.Idle || IsMoving)
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