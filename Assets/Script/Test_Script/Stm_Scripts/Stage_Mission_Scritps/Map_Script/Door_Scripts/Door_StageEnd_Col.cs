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
    [SerializeField] private Transform _endPos;
    [SerializeField] private Collider _col;
    #endregion

    #region 이벤트
    public event Action<Door_StageEnd_Col> OnStageEnd;
    #endregion

    private void Awake()
    {
        if (_col == null)
        {
            if (!TryGetComponent<Collider>(out _col))
            {
                Debug.Log($"[Door_StageEnd_Col] : 콜라이더 없음");
                return;
            }
        }
        if (_endPos == null)
        {
            _endPos = transform;
        }

        _col.isTrigger = false;
    }

    #region 외부 호출 함수
    public void SetTrigger(bool trigger)
    {
        if (_col == null)
        {
            return;
        }

        _col.isTrigger = trigger;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_colTarget))
        {
            return;
        }

        if (other.TryGetComponent<PlayerMover>(out PlayerMover pMover))
        {
            //pMover.RoomClearMoveToDoor(_endPos.position);

            float dis = Vector3.Distance(_endPos.position, pMover.transform.position);
            if (dis <= 0.001f)
            {
                OnStageEnd?.Invoke(this);
            }
        }
    }
}
