using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyCombat

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class EnemyCombat : BaseCombat
{
    #region 내부 변수
    private EnemyState _state;
    #endregion
    protected override void Awake()
    {
        base.Awake();

        _state = GetComponent<EnemyState>();

        if (_state == null)
        {
            Debug.LogError("EnemyCombat _state 참조 실패");
            return;
        }
    }

    void Update()
    {
        if (_state.GetState() == EnemyState.EState.Attack && CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        if (_rangeCheck.TargetTr == null)
        {
            return;
        }

        _lastAtkTime = Time.time;
    }

    public override void OnHitTarget()
    {
        if (_rangeCheck.TargetTr == null)
        {
            return;
        }

        var playerHP = _rangeCheck.TargetTr.GetComponent<HpSystem>();

        if (playerHP != null)
        {
            playerHP.TakeDamage(_status.AtkPower);
        }
    }
}