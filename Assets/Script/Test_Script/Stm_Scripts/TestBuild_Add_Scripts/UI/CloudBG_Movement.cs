using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region 구름 무브먼트
/*
 ▶ 할일
  - 단순하게 일정속도로 움직임

    - 작업자 : 신태민 -
*/
#endregion


public class CloudBG_Movement : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Transform _cloudBg;
    [SerializeField] private float _bgMovementSpeed = 2.0f;
    #endregion

    #region

    #endregion

    private void Awake()
    {
        if (_cloudBg == null)
        {
            _cloudBg = transform;
        }
    }

    private void Update()
    {
        if (_cloudBg == null)
        {
            _cloudBg = transform;
        }

        transform.Translate(Vector2.left * _bgMovementSpeed);
    }
}
