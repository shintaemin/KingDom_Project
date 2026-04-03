using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerCombat

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 플레이어의 자동 공격 수행 및 OnHitTarget 애니메이션 이벤트 함수로 적에게 대미지 전달
*/

public class PlayerCombat : BaseCombat
{
    #region 내부 변수
    public event System.Action OnAttacked;
    #endregion

    void Update()
    {
        if (CanAttack())
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

        LookTarget(_rangeCheck.TargetTr);

        OnAttacked?.Invoke();
    }

    private void LookTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    #region 애니메이션 이벤트 함수
    public override void OnHitTarget()
    {
        if (_rangeCheck.TargetTr == null)
        {
            return;
        }

        var enemyHP = _rangeCheck.TargetTr.GetComponent<HpSystem>();

        if (enemyHP != null)
        {
            enemyHP.TakeDamage(_status.AtkPower);
        }
    }
    #endregion
}