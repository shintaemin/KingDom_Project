using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyCombat

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 적의 상태 기반 자동 공격 및 OnHitTarget 애니메이션 이벤트 함수로 플레이어에게 대미지 전달
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

    protected override void Update()
    {
        if (_state.GetState() == EnemyState.EState.Chase || _state.GetState() == EnemyState.EState.Attack)
        {
            base.Update();
        }

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

        _state.IsAttacking = true;

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

        var playerHP = _rangeCheck.TargetTr.GetComponent<HpSystem>();

        if (playerHP != null)
        {
            playerHP.TakeDamage(_status.AtkPower, transform.position);
        }
    }

    public void EndAttack()
    {
        _state.IsAttacking = false;
    }
    #endregion
}