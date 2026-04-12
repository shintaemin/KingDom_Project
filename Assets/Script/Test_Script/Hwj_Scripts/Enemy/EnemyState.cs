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
        None,
        Idle,
        Patrol,
        Detect,
        Chase,
        ChaseFail,
        Attack,
        Dead,
        BossRoar,
        BossJump,
    }

    #region 인스펙터
    [Header("보스 설정")]
    [SerializeField] private bool _isBoss = false;
    [SerializeField] private float _bossRoarCooldown = 15f;
    #endregion

    #region 내부 변수
    public event System.Action<EState> OnStateChanged;
    public event System.Action<bool> OnDetectionChanged;
    public event System.Action OnDead;

    private EState _state = EState.None;

    private HpSystem _hpSystem;
    private PlayerState _playerState;

    private bool _isDetected = false;

    [HideInInspector]
    public bool IsDetected
    {
        get => _isDetected;

        set
        {
            if (_isDetected != value)
            {
                _isDetected = value;
                OnDetectionChanged?.Invoke(_isDetected);
            }
        }
    }

    [HideInInspector] public bool IsNearDead = false;
    [HideInInspector] public bool IsAtkRange = false;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public bool IsTargetPosArrived = false;
    [HideInInspector] public bool IsOnHit = false;
    [HideInInspector] public bool IsGrounded = true;
    [HideInInspector] public Vector3 DeadPosition;
    private float _chaseTimer = 0f;
    private bool _isPlayerDead = false;
    private float _bossRoarTimer = 0f;
    private bool _canBossRoar = true;
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

    private void OnEnable()
    {
        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged += Damaged;
        }

        if (_playerState == null)
        {
            StartCoroutine(CoBindPlayer());
        }
    }

    private void OnDisable()
    {
        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged -= Damaged;
        }

        if (_playerState != null)
        {
            _playerState.OnDead -= PlayerDead;
        }
    }

    private IEnumerator CoBindPlayer()
    {
        while (_playerState == null)
        {
            _playerState = FindObjectOfType<PlayerState>();
            yield return null;
        }

        _playerState.OnDead += PlayerDead;
    }

    private void Damaged()
    {
        IsOnHit = true;

        if (_isBoss && _canBossRoar)
        {
            _canBossRoar = false;
            SetState(EState.BossRoar);
        }
    }

    private void PlayerDead()
    {
        _isPlayerDead = true;
    }

    void Start()
    {
        SetState(EState.Idle);
    }

    void Update()
    {
        SetState(DecideState());

        if (_isBoss && !_canBossRoar)
        {
            _bossRoarTimer += Time.deltaTime;

            if (_bossRoarTimer >= _bossRoarCooldown)
            {
                _bossRoarTimer = 0f;
                _canBossRoar = true;
            }
        }
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

        _state = next;

        switch (_state)
        {
            case EState.Idle:
                if (_isPlayerDead)
                {
                    break;
                }

                _stateRoutine = StartCoroutine(CoIdleToPatrol(3f));
                break;

            case EState.Patrol:
                IsTargetPosArrived = false;
                break;

            case EState.Detect:
                if (_isBoss && _canBossRoar)
                {
                    _canBossRoar = false;
                    SetState(EState.BossRoar);
                }

                else
                {
                    _stateRoutine = StartCoroutine(CoDetectToChase(1f));
                }

                break;

            case EState.Chase:

                break;

            case EState.ChaseFail:
                DeadPosition = Vector3.zero;
                IsNearDead = false;
                IsOnHit = false;
                SetState(EState.Idle);
                break;

            case EState.Attack:

                break;

            case EState.Dead:
                OnDead?.Invoke();
                gameObject.SetActive(false);
                break;

            case EState.BossRoar:
                _stateRoutine = StartCoroutine(CoBossRoarToJump(2f));
                break;

            case EState.BossJump:
                IsGrounded = false;
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

        if (_isPlayerDead)
        {
            return EState.Idle;
        }

        if (_state == EState.BossRoar || _state == EState.BossJump)
        {
            return _state;
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

        if (IsDetected || IsNearDead || IsOnHit)
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

        IsOnHit = false;
        SetState(EState.Chase);
    }

    private IEnumerator CoIdleToPatrol(float time)
    {
        yield return new WaitForSeconds(time);

        SetState(EState.Patrol);
    }

    private IEnumerator CoBossRoarToJump(float time)
    {
        yield return new WaitForSeconds(time);

        SetState(EState.BossJump);
    }

    #region 외부 호출 함수
    public EState GetState()
    {
        return _state;
    }
    #endregion
}