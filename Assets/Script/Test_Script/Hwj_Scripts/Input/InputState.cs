using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ InputState

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 매 프레임 인풋을 감지하여 상태 결정
*/

public class InputState : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Start,
        Drawing,
        End
    }

    #region 내부 변수
    public event System.Action<EState> OnInputStateChanged;
    private EState _state = EState.Idle;
    private InputReader _input;
    #endregion

    private void Awake()
    {
        _input = GetComponent<InputReader>();

        if (_input == null)
        {
            _input = GetComponentInChildren<InputReader>();

            if (_input == null)
            {
                Debug.LogError("InputState _input 참조 실패");
                return;
            }
        }
    }

    void Update()
    {
        // 매 프레임 터치 감지
        SetInputState(DecideInputState());
    }

    // 단일 상태 진입점
    private void SetInputState(EState next)
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

            case EState.Start:
                Time.timeScale = 0.2f;
                // 물리 연산이 필요하다면 처리 fixedDeltaTime
                break;

            case EState.Drawing:

                break;

            case EState.End:
                Time.timeScale = 1.0f;
                // 물리 연산이 필요하다면 처리 fixedDeltaTime
                break;
        }

        OnInputStateChanged?.Invoke(_state);
    }

    // 상태 결정
    private EState DecideInputState()
    {
        if(_input.GetIsDown())
        {
            return EState.Start;
        }

        if(_input.GetIsUp())
        {
            return EState.End;
        }

        if(_input.GetIsHold())
        {
            return EState.Drawing;
        }
        
        return EState.Idle;
    }

    #region 외부 호출 함수
    public EState GetInputState()
    {
        return _state;
    }
    #endregion
}