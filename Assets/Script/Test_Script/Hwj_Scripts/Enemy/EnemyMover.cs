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
    [SerializeField] private float _detectRange = 5f;

    [Header("추격 설정")]
    [SerializeField] private float _chaseDelay = 0.1f;
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private NavMeshAgent _nav;
    private Transform _playerTr;
    private Coroutine _moveRoutine;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();
        _nav = GetComponent<NavMeshAgent>();

        if (_state == null || _nav == null)
        {
            Debug.LogError("EnemyMover _state _nav 참조 실패");
            return;
        }

        var player = GameObject.FindWithTag(_playerTag);

        if (player != null)
        {
            _playerTr = player.transform;

            if (_playerTr == null)
            {
                Debug.LogError("EnemyMover _playerTr 참조 실패 (태그 설정 필요)");
                return;
            }
        }
    }

    private void OnEnable()
    {
        if (_state != null)
        {
            _state.OnStateChanged += StateChanged;
        }
    }

    private void OnDisable()
    {
        if (_state != null)
        {
            _state.OnStateChanged -= StateChanged;
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
                _nav.speed = 1.5f;
                _nav.isStopped = false;
                _moveRoutine = StartCoroutine(CoPatrol());
                break;

            case EnemyState.EState.Chase:
                _nav.speed = 2.2f;
                _nav.isStopped = false;
                _moveRoutine = StartCoroutine(CoChase());
                break;
        }
    }

    private IEnumerator CoPatrol()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * _detectRange;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _detectRange, NavMesh.AllAreas))
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
}