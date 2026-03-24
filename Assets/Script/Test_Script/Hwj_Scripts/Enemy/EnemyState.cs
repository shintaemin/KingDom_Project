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

    [HideInInspector] public bool IsNearDead = false;
    [HideInInspector] public bool IsDetected = false;
    private float _chaseTimer = 0f;
    private Coroutine _stateRoutine;
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

        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
            _stateRoutine = null;
        }

        _state = next;

        switch (_state)
        {
            case EState.None:

                break;

            case EState.Patrol:

                break;

            case EState.Detect:
                _stateRoutine = StartCoroutine(CoDetectToChase());
                break;

            case EState.Chase:

                break;

            case EState.ChaseFail:
                _stateRoutine = StartCoroutine(CoChaseFailToPatrol());
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

        if (_state == EState.Detect || _state == EState.ChaseFail)
        {
            return _state;
        }

        if (_state == EState.Chase)
        {
            if (IsDetected)
            {
                _chaseTimer = 0f;
                return EState.Chase;
            }

            else
            {
                _chaseTimer += Time.deltaTime;

                if (_chaseTimer >= 5f)
                {
                    _chaseTimer = 0f;
                    return EState.ChaseFail;
                }

                return EState.Chase;
            }
        }

        if (IsNearDead || IsDetected) 
        {
            IsNearDead = false;

            return EState.Detect;
        }

        return EState.Patrol;
    }

    private IEnumerator CoDetectToChase()
    {
        yield return new WaitForSeconds(0.5f);

        SetState(EState.Chase);
    }

    private IEnumerator CoChaseFailToPatrol()
    {
        yield return new WaitForSeconds(2f);

        SetState(EState.Patrol);
    }

    #region 외부 호출 함수
    public EState GetState()
    {
        return _state;
    }
    #endregion
}