using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 로딩 배경 스크롤
/*
 ▶ 할일
  - 로딩 화면의 배경 이미지를 일정 방향으로 이동
  - RectTransform 기반으로 UI 위치를 갱신
  - 이동 속도와 방향을 인스펙터에서 제어

 ※ 참고사항
  - Update에서 매 프레임 위치 이동 처리
  - 방향 벡터(_direction)와 속도(_moveSpeed)를 곱하여 이동량 계산

  - 박라희
*/
#endregion

public class Loading_sky_BG : MonoBehaviour
{
    #region 인스펙터
    [Header("이동 속도")]
    [SerializeField] private float _moveSpeed = 30f;

    [Header("이동 방향")]
    [SerializeField] private Vector2 _direction = Vector2.left;
    #endregion

    #region 내부 변수
    private RectTransform _rectTr;
    #endregion

    private void Awake()
    {
        // RectTransform 캐싱
        _rectTr = GetComponent<RectTransform>();
    }

    private void Update()
    {
        MoveBackground();
    }

    #region 내부 함수
    // 배경 위치 이동
    private void MoveBackground()
    {
        if (_rectTr == null)
        {
            return;
        }

        // 방향 * 속도 * 시간 만큼 위치 이동 
        _rectTr.anchoredPosition += _direction * _moveSpeed * Time.deltaTime;
    }
    #endregion

}
