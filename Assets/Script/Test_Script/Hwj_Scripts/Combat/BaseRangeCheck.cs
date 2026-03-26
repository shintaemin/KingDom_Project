using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ BaseRangeCheck

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 공격 사거리를 체크하는 기본 기반 클래스 → 타겟과 거리를 계산하여 공격 사거리 판단
*/

public abstract class BaseRangeCheck : MonoBehaviour
{
    #region 내부 변수
    protected BaseStatus _status;
    protected Transform _targetTr;
    protected bool _isAtkRange = false;
    #endregion

    #region 프로퍼티
    [HideInInspector] public bool IsAtkRange => _isAtkRange;
    #endregion

    protected virtual void Awake()
    {
        _status = GetComponent<BaseStatus>();

        if (_status == null)
        {
            Debug.LogError("BaseRangeCheck _status 참조 실패");
            return;
        }
    }

    protected virtual void Update()
    {
        UpdateTarget();
        DistanceCheck();
    }

    protected void DistanceCheck()
    {
        if (_targetTr == null)
        {
            _isAtkRange = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, _targetTr.position);

        if (distance <= _status.AtkRange)
        {
            _isAtkRange = true;
        }

        else
        {
            _isAtkRange = false;
        }
    }

    protected abstract void UpdateTarget();
}