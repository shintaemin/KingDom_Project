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
    [Header("거리 설정")]
    [SerializeField] private float _arrivedEnemyDistance = 0.5f;

    [Header("기본 이동속도 설정")]
    [SerializeField] private float _baseSpeed = 5f;
    #endregion

    #region 내부 변수
    private NavMeshAgent _nav;
    private InputState _inputState;
    private PlayerState _playerState;
    private PlayerPathRecorder _pathRecorder;
    private Coroutine _moveRoutine;
    #endregion

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _inputState = GetComponent<InputState>();
        _playerState = GetComponent<PlayerState>();
        _pathRecorder = GetComponent<PlayerPathRecorder>();

        if (_nav == null || _inputState == null || _playerState == null || _pathRecorder == null)
        {
            Debug.LogError("PlayerMover _nav _inputState _playerState _pathRecorder 참조 실패");
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
            if (Vector3.Distance(transform.position, enemy.position) <= _arrivedEnemyDistance)
            {
                _playerState.IsMoving = false;
                _nav.ResetPath();
                break;
            }

            else
            {
                _nav.SetDestination(enemy.position);
            }

            yield return null;
        }

        _playerState.IsMoving = false;
    }

    private IEnumerator MoveToPath()
    {
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

    #region 외부 호출 함수
    public void SetMoveSpeed(float speed)
    {
        if (_nav == null)
        {
            _nav = GetComponent<NavMeshAgent>();
        }

        _nav.speed = _baseSpeed * (speed / 100f);

        _nav.acceleration = _baseSpeed * 10f * (speed / 100f);
    }
    #endregion
}