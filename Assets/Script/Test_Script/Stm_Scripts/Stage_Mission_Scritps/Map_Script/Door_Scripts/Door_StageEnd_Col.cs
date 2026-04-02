using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 스테이지 종료 충돌
/*
 ▶ 할일
  - 플레이어 충돌을 감지
  - 충돌 이벤트 발행 -> 인게임매니저가 이 이벤트를 구독하고 다음맵을 불러온다.
*/
#endregion


public class Door_StageEnd_Col : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _colTarget = "Player";
    #endregion

    #region 이벤트
    public event Action<Door_StageEnd_Col> OnStageEnd;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_colTarget))
        {
            return;
        }

        OnStageEnd.Invoke(this);
    }
}
