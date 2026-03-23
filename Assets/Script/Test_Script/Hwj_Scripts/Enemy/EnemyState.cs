using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyState

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 매 프레임 정보를 받아 적 상태 결정
*/

public class EnemyState : MonoBehaviour
{
    public enum EState
    {
        None, // 스테이지 시작시에 몹이 제자리에 있어야 할 경우가 필요하다면
        Patrol,
        Detect,
        Chase,
        ChaseFail,
        Attack,
        Dead,
    }

    #region 내부 변수
    public event System.Action<EState> OnStateChanged;
    public event System.Action OnDead;

    private EState _state = EState.None;

    public bool IsNearDead = false;
    public bool IsDetected = false;
    #endregion

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
            case EState.None:

                break;

            case EState.Patrol:

                break;

            case EState.Detect:
                // 1초뒤에 Chase상태로 변경
                break;

            case EState.Chase:

                break;

            case EState.ChaseFail:

                break;

            case EState.Attack:

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
        //if () Status 에서 HP = 0 일경우 IsDead를 받아와서 조건문에 넣으면 될 듯???
        //{
        //    return EState.Dead;
        //}
        //
        //if () 플레이어가 공격 사거리 안에 들어왔나?
        //{
        //    return EState.Attack;
        //}
        //
        //if () 추격 중에 5초동안 플레이어를 못찾았으면?
        //{
        //    return EState.ChaseFail;
        //}
        //
        //if () 공격받았을때
        //{
        //    return EState.Chase;
        //}
        //
        if (IsNearDead || IsDetected) 
        {
            IsNearDead = false;

            return EState.Detect;
        }

        return EState.Patrol;
    }

    #region 외부 호출 함수
    public EState GetState()
    {
        return _state;
    }
    #endregion
}