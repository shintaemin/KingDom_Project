using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerPathRenderer

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class PlayerPathRenderer : MonoBehaviour
{
    #region 인스펙터
    [Header("y축 보정값")]
    [SerializeField] private float _yOffset = 0.1f;
    #endregion

    #region 내부 변수
    private LineRenderer _lineRenderer;
    private PlayerMover _mover;
    #endregion

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _mover = GetComponent<PlayerMover>();

        if (_lineRenderer == null || _mover == null)
        {
            Debug.LogError("PlayerPathRenderer _lineRenderer _mover 참조 실패");
            return;
        }
    }

    void Update()
    {
        DrawPath();
    }

    private void DrawPath()
    {

    }
}