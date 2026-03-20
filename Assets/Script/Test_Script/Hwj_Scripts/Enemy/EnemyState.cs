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
        Dead,
    }

    #region 내부 변수
    public event System.Action<EState> OnEnemyStateChanged;
    public event System.Action<bool> OnDead;
    
    private EState _state = EState.None;
    private bool _isDead = true;
    #endregion

    void Update()
    {
        SetEnemyState(DecideEnemyState());
    }

    // 단일 상태 진입점
    private void SetEnemyState(EState next)
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

                break;

            case EState.Chase:

                break;

            case EState.Dead:
                _isDead = true;
                OnDead?.Invoke(_isDead);
                break;
        }

        OnEnemyStateChanged?.Invoke(_state);
    }

    // 상태 결정
    private EState DecideEnemyState()
    {
        //if () Status 에서 HP = 0 일경우 IsDead를 받아와서 조건문에 넣으면 될 듯???
        //{
        //    return EState.Dead;
        //}
        //
        //if () 
        //{
        //    return EState.Chase;
        //}
        //
        //if () 시야 범위안에 들어올 경우 1초 이후에 Chase로 전환
        //{
        //    return EState.Detect;
        //}

        return EState.Patrol;
    }

    #region 외부 호출 함수
    public EState GetEnemyState()
    {
        return _state;
    }
    #endregion
}