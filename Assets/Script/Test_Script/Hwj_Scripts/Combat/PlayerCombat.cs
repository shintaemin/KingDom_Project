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
    protected override void Update()
    {
        base.Update();

        if (CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        base.Attack();
    }

    #region 애니메이션 이벤트 함수
    public override void OnHitTarget()
    {
        if (_rangeCheck.TargetTr == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _rangeCheck.TargetTr.position);

        if (distance > _status.AtkRange)
        {
            // 공격 사거리 밖에 있으면 대미지 X
            return;
        }

        Vector3 direction = (_rangeCheck.TargetTr.position - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, direction);

        if (dot < 0.5f)
        {
            // 정면이 아니면 대미지 X
            return;
        }

        var enemyHP = _rangeCheck.TargetTr.GetComponent<HpSystem>();

        if (enemyHP != null)
        {
            enemyHP.TakeDamage(_status.AtkPower, transform.position);
        }
    }
    #endregion
}