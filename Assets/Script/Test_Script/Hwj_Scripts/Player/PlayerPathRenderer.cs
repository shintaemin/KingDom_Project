using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerPathRenderer

    ㆍ 작성자 : 황원준

    ㆍ 기능 : NavMeshAgent의 경로 데이터를 시각화
*/

public class PlayerPathRenderer : MonoBehaviour
{
    #region 인스펙터
    [Header("y축 보정값")]
    [SerializeField] private float _yOffset = 0.1f;
    #endregion

    #region 내부 변수
    private LineRenderer _lineRenderer;
    private NavMeshAgent _nav;
    #endregion

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _nav = GetComponent<NavMeshAgent>();

        if (_lineRenderer == null || _nav == null)
        {
            Debug.LogError("PlayerPathRenderer _lineRenderer _nav 참조 실패");
            return;
        }
    }

    void Update()
    {
        if (_nav.hasPath)
        {
            DrawPath();
        }
    }

    private void DrawPath()
    {
        Vector3[] points = _nav.path.corners;

        if (points.Length < 2)
        {
            return;
        }

        _lineRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 position = points[i];

            position.y += _yOffset;

            _lineRenderer.SetPosition(i, position);
        }

        _lineRenderer.alignment = LineAlignment.View;
    }
}