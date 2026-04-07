using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyRangedCombat

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 적의 상태 기반 자동 공격 및 OnHitTarget 애니메이션 이벤트 함수로 플레이어에게 대미지 전달
*/

public class EnemyRangedCombat : BaseCombat
{
    #region 인스펙터
    [Header("투사체 설정")]
    [SerializeField] private ProjectileFactory.ProjectileType _projectileType;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _forwardOffset = 0.2f;
    #endregion

    #region 내부 변수
    private EnemyState _state;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        _state = GetComponent<EnemyState>();

        if (_state == null)
        {
            Debug.LogError("EnemyRangedCombat _state 참조 실패");
            return;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_state.GetState() == EnemyState.EState.Attack && CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack()
    {
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

        // 플레이어가 투사체에 맞았을 시. 즉 트리거 스크립트? 에서 플레이어가 맞으면 디스폰하고 대미지 주면 될듯?
        //var playerHp = _rangeCheck.TargetTr.GetComponent<HpSystem>();
        //
        //if (playerHp != null)
        //{
        //    playerHp.TakeDamage(_status.AtkPower, transform.position);
        //}
    }

    public void EndAttack()
    {
        _state.IsAttacking = false;
    }
    #endregion
}