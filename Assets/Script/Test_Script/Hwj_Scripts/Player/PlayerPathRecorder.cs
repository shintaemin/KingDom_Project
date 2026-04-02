using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerPathRecorder

    ㆍ 작성자 : 황원준

    ㆍ 기능 : Input을 통해 플레이어가 이동할 경로 및 대상 기록
*/

public class PlayerPathRecorder : MonoBehaviour
{
    #region 인스펙터
    [Header("레이어 설정")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private LayerMask _notTerrainLayer;
    [SerializeField] private LayerMask _playerLayer;
    #endregion

    #region 내부 변수
    private List<Vector3> _wayPoints = new List<Vector3>();
    private InputReader _inputReader;
    private InputState _inputState;
    private PlayerState _playerState;
    private Camera _camera;
    private NavMeshAgent _nav;
    private Transform _enemyTr;
    #endregion

    private void Awake()
    {
        _camera = Camera.main;
        _inputReader = GetComponent<InputReader>();
        _inputState = GetComponent<InputState>();
        _playerState = GetComponent<PlayerState>();
        _nav = GetComponent<NavMeshAgent>();

        if (_inputReader == null)
        {
            Debug.LogError("PlayerPathRecorder _inputReader _inputState _playerState _nav 참조 실패");
            return;
        }
    }

    void Update()
    {
        if (_inputState.GetState() == InputState.EState.Drawing)
        {
            DrawingRecordPath();
        }
    }

    private void DrawingRecordPath()
    {
        Ray ray = _camera.ScreenPointToRay(_inputReader.GetInputPosition());

        if (Physics.Raycast(ray, out RaycastHit hitTerrain, Mathf.Infinity, _terrainLayer))
        {
            Vector3 center = hitTerrain.transform.position;

            center.y = transform.position.y;

            if (Vector3.Distance(transform.position, center) < 0.5f)
            {
                ResetPath();
                return;
            }

            // 이미 웨이포인트에 존재하는 위치일 경우 해당 위치 웨이포인트 제거
            if (_wayPoints.Contains(center))
            {
                int index = _wayPoints.IndexOf(center);

                if (index < _wayPoints.Count - 1)
                {
                    _wayPoints.RemoveRange(index + 1, _wayPoints.Count - (index + 1));
                }
            }

            // 새로운 웨이포인트일 경우 추가
            else
            {
                _wayPoints.Add(center);
            }
        }
    }

    #region 외부 호출 함수
    public void ClickRecordPath()
    {
        Ray ray = _camera.ScreenPointToRay(_inputReader.GetInputPosition());

        if (Physics.Raycast(ray, out RaycastHit hitPlayer, Mathf.Infinity, _playerLayer) ||
            Physics.Raycast(ray, out RaycastHit hitNotTerrain, Mathf.Infinity, _notTerrainLayer))
        {
            _playerState.IsMoving = false;
            _nav.ResetPath();
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hitEnemy, Mathf.Infinity, _enemyLayer))
        {
            _enemyTr = hitEnemy.transform;

            Vector3 enemy = hitEnemy.transform.position;

            enemy.y = transform.position.y;

            _wayPoints.Add(enemy);
        }

        else if (Physics.Raycast(ray, out RaycastHit hitTerrain, Mathf.Infinity, _terrainLayer))
        {
            Vector3 center = hitTerrain.transform.position;

            center.y = transform.position.y;

            _wayPoints.Add(center);
        }
    }

    public void ResetPath()
    {
        _wayPoints.Clear();
        _enemyTr = null;
    }

    public List<Vector3> GetPath()
    {
        return _wayPoints;
    }

    public Transform GetEnemy()
    {
        return _enemyTr;
    }
    #endregion
}