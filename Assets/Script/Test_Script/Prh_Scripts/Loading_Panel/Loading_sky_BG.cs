using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_sky_BG : MonoBehaviour
{
    [Header("이동 속도")]
    [SerializeField] private float _moveSpeed = 30f;

    [Header("이동 방향 (왼쪽)")]
    [SerializeField] private Vector2 _direction = Vector2.left;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 오른쪽 → 왼쪽 이동
        _rect.anchoredPosition += _direction * _moveSpeed * Time.deltaTime;
    }

}
