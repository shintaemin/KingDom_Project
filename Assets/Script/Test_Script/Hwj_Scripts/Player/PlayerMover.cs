using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerMover

    ㆍ 작성자 : 황원준

    ㆍ 기능 : InputState의 상태에 따라 경로를 기록하고 NavMesh로 이동
*/

public class PlayerMover : MonoBehaviour
{
    #region 인스펙터
    [Header("레이어 설정")]
    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("거리 설정")]
    [SerializeField] private float _arrivedDistance = 0.5f;
    [SerializeField] private float _pathDistanceOffset = 0.1f;
    #endregion

    #region 내부 변수
    private List<Vector3> _wayPoints = new List<Vector3>();
    private NavMeshAgent _nav;
    private Camera _camera;
    private InputReader _inputReader;
    private InputState _inputState;
    private Coroutine _moveRoutine;
    private Transform _enemyTr;
    #endregion

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _inputReader = GetComponent<InputReader>();
        _inputState = GetComponent<InputState>();
        _camera = Camera.main;

        if (_nav == null || _inputReader == null || _inputState == null)
        {
            Debug.LogError("PlayerMover _nav _inputReader _inputState 참조 실패");
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

    void Update()
    {
        if (_inputState.GetState() == InputState.EState.Drawing)
        {
            RecordPath();
        }
    }

    private void StateChanged(InputState.EState state)
    {
        switch (state)
        {
            case InputState.EState.Start:
                ResetPath();
                CheckTouchEnemy();
                break;

            case InputState.EState.End:
                StartMove();
                break;
        }
    }

    private void ResetPath()
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        _wayPoints.Clear();
        _nav.ResetPath();
        _enemyTr = null;
    }

    private void CheckTouchEnemy()
    {
        Ray ray = _camera.ScreenPointToRay(_inputReader.GetMousePosition());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _enemyLayer))
        {
            _enemyTr = hit.transform;
        }
    }

    private void RecordPath()
    {
        Ray ray = _camera.ScreenPointToRay(_inputReader.GetMousePosition());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _terrainLayer))
        {
            Vector3 center = hit.transform.position;

            if (_wayPoints.Count == 0 || _wayPoints[_wayPoints.Count - 1] !=  center)
            {
                _wayPoints.Add(center);
                _enemyTr = null;
            }
        }
    }

    private void StartMove()
    {
        if (_enemyTr != null || _wayPoints.Count > 0)
        {
            _moveRoutine = StartCoroutine(CoPathRoutine());
        }
    }

    private IEnumerator CoPathRoutine()
    {
        if (_enemyTr != null)
        {
            while (_enemyTr != null)
            {
                _nav.SetDestination(_enemyTr.position);

                if (Vector3.Distance(transform.position, _enemyTr.position) < _arrivedDistance)
                {
                    _nav.ResetPath();
                    break;
                }

                yield return null;
            }
        }

        else
        {
            while (_wayPoints.Count > 0)
            {
                Vector3 position = _wayPoints[0];
                _nav.SetDestination(position);

                while (_nav.pathPending || _nav.remainingDistance > _pathDistanceOffset)
                {
                    yield return null;
                }

                _wayPoints.RemoveAt(0);
            }
        }

        _moveRoutine = null;
    }
}