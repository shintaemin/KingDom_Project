using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ BaseCombat

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 플레이어, 적 공통 공격 로직 (사거리 체크, 공격 쿨타임 체크 등)
*/

public abstract class BaseCombat : MonoBehaviour
{
    #region 내부 변수
    protected BaseStatus _status;
    protected BaseRangeCheck _rangeCheck;
    protected float _lastAtkTime;
    #endregion

    protected virtual void Awake()
    {
        _status = GetComponent<BaseStatus>();
        _rangeCheck = GetComponent<BaseRangeCheck>();

        if (_status == null || _rangeCheck == null)
        {
            Debug.LogError("BaseCombat _status _rangeCheck 참조 실패");
            return;
        }
    }

    protected virtual bool CanAttack()
    {
        if (_rangeCheck == null || !_rangeCheck.IsAtkRange)
        {
            return false;
        }

        if (Time.time - _lastAtkTime < _status.AtkSpeed)
        {
            return false;
        }

        return true;
    }

    protected abstract void Attack();
    public abstract void OnHitTarget();
}