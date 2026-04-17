using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ EnemyMover

    ㆍ 작성자 : 황원준

    ㆍ 기능 : EnemyState의 상태에 따라 NavMesh로 이동
*/

public class EnemyMover : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어 태그")]
    [SerializeField] private string _playerTag = "Player";

    [Header("정찰 설정")]
    [SerializeField] private float _detectMoveRange = 5f;

    [Header("추격 설정")]
    [SerializeField] private float _chaseDelay = 0.1f;

    [Header("보스 점프 설정")]
    [SerializeField] private float _jumpMinRange = 3f;
    [SerializeField] private float _jumpMaxRange = 7f;
    [SerializeField] private float _jumpDuration = 1f;
    [SerializeField] private float _jumpHeight = 3f;
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private NavMeshAgent _nav;
    private Transform _playerTr;
    private Coroutine _moveRoutine;
    private BaseStatus _baseStatus;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();
        _nav = GetComponent<NavMeshAgent>();
        _baseStatus = GetComponent<BaseStatus>();

        if (_state == null || _nav == null || _baseStatus == null)
        {
            Debug.LogError("EnemyMover _state _nav 참조 실패");
            return;
        }

        //var player = GameObject.FindWithTag(_playerTag);
        //
        //if (player != null)
        //{
        //    _playerTr = player.transform;
        //
        //    if (_playerTr == null)
        //    {
        //        Debug.LogError("EnemyMover _playerTr 참조 실패 (태그 설정 필요)");
        //        return;
        //    }
        //}
    }

    private void OnEnable()
    {
        if (_state != null)
        {
            _state.OnStateChanged += StateChanged;
        }

        StartCoroutine(CoBindPlayer());
    }

    private void OnDisable()
    {
        if (_state != null)
        {
            _state.OnStateChanged -= StateChanged;
        }
    }

    private IEnumerator CoBindPlayer()
    {
        while (_playerTr == null)
        {
            var player = GameObject.FindWithTag(_playerTag);

            if (player != null)
            {
                _playerTr = player.transform;
            }

            else
            {
                yield return null;
            }
        }
    }

    private void StateChanged(EnemyState.EState state)
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        _nav.isStopped = true;
        _nav.ResetPath();

        switch (state)
        {
            case EnemyState.EState.Patrol:
                _nav.speed = _baseStatus.MoveSpeed;
                _nav.isStopped = false;
                _moveRoutine = StartCoroutine(CoPatrol());
                break;

            case EnemyState.EState.Chase:
                _nav.speed = _baseStatus.MoveSpeed * 1.5f;
                _nav.isStopped = false;
                _moveRoutine = StartCoroutine(CoChase());
                break;

            case EnemyState.EState.BossJump:
                _nav.enabled = false;
                _moveRoutine = StartCoroutine(CoBossJump());
                break;
        }
    }

    private IEnumerator CoPatrol()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * _detectMoveRange;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _detectMoveRange, NavMesh.AllAreas))
        {
            Vector3 center = hit.position;

            center.y = transform.position.y;

            _nav.SetDestination(center);

            while (_nav.pathPending || _nav.remainingDistance > _nav.stoppingDistance)
            {
                yield return null;
            }

            _state.IsTargetPosArrived = true;
        }
    }

    private IEnumerator CoChase()
    {
        while (_state.GetState() == EnemyState.EState.Chase)
        {
            if (_state.DeadPosition != Vector3.zero)
            {
                _nav.SetDestination(_state.DeadPosition);
            }

            else
            {
                _nav.SetDestination(_playerTr.position);
            }

            yield return new WaitForSeconds(_chaseDelay);
        }
    }

    private IEnumerator CoBossJump()
    {
        Vector3 dir = Random.onUnitSphere;
        dir.y = 0;
        dir.Normalize();

        Vector3 randomPos = _playerTr.position + (dir * Random.Range(_jumpMinRange, _jumpMaxRange));

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _jumpMaxRange, NavMesh.AllAreas))
        {
            Vector3 center = hit.position;

            center.y = transform.position.y;

            transform.rotation = Quaternion.LookRotation(dir);

            Vector3 startPos = transform.position;
            float timer = 0;

            while (_jumpDuration > timer)
            {
                timer += Time.deltaTime;

                float t = timer / _jumpDuration;

                // 수평
                Vector3 movePos = Vector3.Lerp(startPos, center, t);

                // 수직
                movePos.y += Mathf.Sin(t * Mathf.PI) * _jumpHeight;

                transform.position = movePos;

                yield return null;
            }

            transform.position = center;
            _nav.Warp(center);
            _nav.enabled = true;
            _state.IsGrounded = true;
            EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.BossJumpEnd, transform.position, transform.rotation);
        }
    }
}