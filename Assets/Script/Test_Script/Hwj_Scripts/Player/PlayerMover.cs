using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerMover

    ㆍ 작성자 : 황원준

    ㆍ 기능 : PlayerPathRecorder에 기록된 데이터를 기반으로 플레이어 이동 제어
*/

public class PlayerMover : MonoBehaviour
{
    #region 인스펙터
    [Header("적 선택시 도착 거리 설정 (보정값느낌)")]
    [SerializeField] private float _arrivedEnemyDistance = 0.5f;

    [Header("백어택 설정")]
    [SerializeField] private float _backAttackDistance = 4f;
    [SerializeField] private float _backAttackSpeedMultiplier = 1.5f;
    #endregion

    #region 내부 변수
    private NavMeshAgent _nav;
    private InputState _inputState;
    private PlayerState _playerState;
    private PlayerPathRecorder _pathRecorder;
    private BaseStatus _status;
    private Coroutine _moveRoutine;
    #endregion

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _inputState = GetComponent<InputState>();
        _playerState = GetComponent<PlayerState>();
        _pathRecorder = GetComponent<PlayerPathRecorder>();
        _status = GetComponent<BaseStatus>();

        if (_nav == null || _inputState == null || _playerState == null || _pathRecorder == null || _status == null)
        {
            Debug.LogError("PlayerMover _nav _inputState _playerState _pathRecorder _status 참조 실패");
            return;
        }
    }

    private void OnEnable()
    {
        if (_inputState != null)
        {
            _inputState.OnStateChanged += StateChanged;
        }
    }

    private void OnDisable()
    {
        if (_inputState != null)
        {
            _inputState.OnStateChanged -= StateChanged;
        }
    }

    private void StateChanged(InputState.EState state)
    {
        switch (state)
        {
            case InputState.EState.Start:
                _pathRecorder.ResetPath();
                StopMove();
                _pathRecorder.ClickRecordPath();
                break;

            case InputState.EState.Drawing:
                _nav.ResetPath();
                _playerState.IsMoving = true; // 이게 작동을 안함.. 아마 속도 때문인듯
                break;

            case InputState.EState.End:
                StartMove();
                break;
        }
    }

    private void StartMove()
    {
        Transform enemy = _pathRecorder.GetEnemy();

        if (enemy != null)
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }

            _moveRoutine = StartCoroutine(MoveToEnemy(enemy));
            _playerState.IsMoving = true;
        }

        else if (_pathRecorder.GetPath().Count > 0)
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }

            _moveRoutine = StartCoroutine(MoveToPath());
            _playerState.IsMoving = true;
        }
    }

    private void StopMove()
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }
    }

    private IEnumerator MoveToEnemy(Transform enemy)
    {
        while (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);

            if (distance <= _arrivedEnemyDistance)
            {
                _playerState.IsMoving = false;
                _nav.ResetPath();
                SetMoveSpeed(100f);
            }

            else
            {
                _nav.SetDestination(enemy.position);

                _playerState.IsMoving = true;

                Vector3 direction = (transform.position - enemy.position).normalized;

                float dot = Vector3.Dot(enemy.forward, direction);

                if (distance <= _backAttackDistance && dot < -0.5f)
                {
                    SetMoveSpeed(100f * _backAttackSpeedMultiplier);
                }

                else
                {
                    SetMoveSpeed(100f);
                }
            }

            yield return null;
        }

        _playerState.IsMoving = false;
        SetMoveSpeed(100f);
    }

    private IEnumerator MoveToPath()
    {
        SetMoveSpeed(100f);

        List<Vector3> path = _pathRecorder.GetPath();

        while (path.Count > 0)
        {
            _nav.SetDestination(path[0]);

            if (Vector3.Distance(transform.position, path[0]) <= _nav.stoppingDistance)
            {
                path.RemoveAt(0);
            }

            yield return null;
        }

        _playerState.IsMoving = false;
    }

    private void SetMoveSpeed(float speed)
    {
        if (_nav == null)
        {
            _nav = GetComponent<NavMeshAgent>();
        }

        _nav.speed = _status.MoveSpeed * (speed / 100f);

        _nav.acceleration = _status.MoveSpeed * 10f * (speed / 100f);
    }

    #region 외부 호출 함수
    public void RoomClearMoveToDoor(Vector3 targetPosition)
    {
        StopMove();

        if (_nav == null)
        {
            _nav = GetComponent<NavMeshAgent>();
        }

        _nav.SetDestination(targetPosition);

        _playerState.IsMoving = true;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _backAttackDistance);
    }
}