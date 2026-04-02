using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerPathRenderer

    ㆍ 작성자 : 황원준

    ㆍ 기능 : PlayerPathRecorder에 저장된 경로 및 대상을 기반으로 LineRenderer로 시각화
*/

public class PlayerPathRenderer : MonoBehaviour
{
    #region 인스펙터
    [Header("y축 보정값")]
    [SerializeField] private float _yOffset = 0.1f;
    #endregion

    #region 내부 변수
    private LineRenderer _lineRenderer;
    private PlayerPathRecorder _pathRecorder;
    private NavMeshPath _nav;
    private List<Vector3> _pathPoint;
    #endregion

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _pathRecorder = GetComponent<PlayerPathRecorder>();
        _nav = new NavMeshPath();
        _pathPoint = new List<Vector3>(100);

        if (_lineRenderer == null)
        {
            Debug.LogError("PlayerPathRenderer _lineRenderer _pathRecorder 참조 실패");
            return;
        }
    }

    void Update()
    {
        DrawPath();
    }

    private void DrawPath()
    {
        List<Vector3> path = _pathRecorder.GetPath();
        Transform enemy = _pathRecorder.GetEnemy();

        if ((path == null || path.Count == 0) && enemy == null)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        _pathPoint.Clear();

        Vector3 currentPos = transform.position;

        if (enemy != null)
        {
            if (NavMesh.CalculatePath(currentPos, enemy.position, NavMesh.AllAreas, _nav))
            {
                for (int i = 0; i < _nav.corners.Length; i++)
                {
                    _pathPoint.Add(_nav.corners[i]);
                }
            }
        }

        else
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (NavMesh.CalculatePath(currentPos, path[i], NavMesh.AllAreas, _nav))
                {
                    for (int j = 0; j < _nav.corners.Length; j++)
                    {
                        _pathPoint.Add(_nav.corners[j]);
                    }

                    currentPos = path[i];
                }
            }
        }

        _lineRenderer.positionCount = _pathPoint.Count;

        for (int i = 0; i < _pathPoint.Count; i++)
        {
            _lineRenderer.SetPosition(i, _pathPoint[i] + Vector3.up * _yOffset);
        }
    }
}