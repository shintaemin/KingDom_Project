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
        Idle,
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

    private EState _state = EState.Patrol;

    private HpSystem _hpSystem;
    
    [HideInInspector] public bool IsNearDead = false;
    [HideInInspector] public bool IsDetected = false;
    [HideInInspector] public bool IsAtkRange = false;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public bool IsTargetPosArrived = false;
    [HideInInspector] public Vector3 DeadPosition;
    private float _chaseTimer = 0f;
    private Coroutine _stateRoutine;
    #endregion

    private void Awake()
    {
        _hpSystem = GetComponent<HpSystem>();

        if (_hpSystem == null)
        {
            Debug.LogError("EnemyState _hpSystem 참조 실패");
            return;
        }
    }

    void Start()
    {
        SetState(EState.Idle);
    }

    void Update()
    {
        SetState(DecideState());
    }

    // 단일 상태 진입점
    private void SetState(EState next)
    {
        if (_state == EState.Dead || _state == next)
        {
            return;
        }

        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
            _stateRoutine = null;
        }

        //Debug.Log($"EnemyState : {_state} -> {next}");

        _state = next;

        switch (_state)
        {
            case EState.Idle:
                _stateRoutine = StartCoroutine(CoIdleToPatrol(2f));
                break;

            case EState.Patrol:
                IsTargetPosArrived = false;
                break;

            case EState.Detect:
                if (IsNearDead)
                {
                    IsNearDead = false;
                }

                _stateRoutine = StartCoroutine(CoDetectToChase(1f));
                break;

            case EState.Chase:

                break;

            case EState.ChaseFail:
                DeadPosition = Vector3.zero;
                SetState(EState.Idle);
                break;

            case EState.Attack:
                
                break;

            case EState.Dead:
                OnDead?.Invoke();
                gameObject.SetActive(false);
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

        if (_state == EState.Attack)
        {
            if (IsAttacking)
            {
                return EState.Attack;
            }

            if (!IsAtkRange)
            {
                return EState.Chase;
            }

            _chaseTimer = 0f;
            return EState.Attack;
        }

        if (_state == EState.Chase)
        {
            if (IsAtkRange)
            {
                return EState.Attack;
            }

            if (IsDetected)
            {
                _chaseTimer = 0f;
            }

            else
            {
                _chaseTimer += Time.deltaTime;

                if (_chaseTimer >= 5f)
                {
                    _chaseTimer = 0f;
                    return EState.ChaseFail;
                }
            }

            return EState.Chase;
        }

        if (IsDetected || IsNearDead)
        {
            return EState.Detect;
        }

        if (_state == EState.Patrol)
        {
            if (IsTargetPosArrived)
            {
                return EState.Idle;
            }

            return EState.Patrol;
        }

        return _state;
    }

    private IEnumerator CoDetectToChase(float time)
    {
        yield return new WaitForSeconds(time);

        SetState(EState.Chase);
    }

    private IEnumerator CoIdleToPatrol(float time)
    {
        yield return new WaitForSeconds(time);

        SetState(EState.Patrol);
    }

    #region 외부 호출 함수
    public EState GetState()
    {
        return _state;
    }
    #endregion
}