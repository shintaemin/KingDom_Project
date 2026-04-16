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
    [SerializeField] private ProjectileManager.EProjectileType _projectileType;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _forwardOffset = 0.2f;
    [SerializeField] private float _speed = 500f;

    [Header("정면 자동공격 (18 스테이지 전용)")]
    [SerializeField] private bool _autoAttack = false;
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private string _playerTag = "Player";
    private Transform _playerTr;
    #endregion
    private IEnumerator CoBindPlayer()
    {
        while (_playerTr == null)
        {
            var player = GameObject.FindWithTag(_playerTag);

            if (player != null)
            {
                Debug.Log("플레이어 태그 지정 완료");
                _playerTr = player.transform;
                yield break;
            }

            else
            {
                yield return null;
            }
        }
    }

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

    private void OnEnable()
    {
        StartCoroutine(CoBindPlayer());
    }

    protected override void Update()
    {
        if (!_autoAttack)
        {
            if (_state.GetState() == EnemyState.EState.Chase || _state.IsAttacking)
            {
                base.Update();
            }

            if (_state.GetState() == EnemyState.EState.Attack && CanAttack())
            {
                Attack();
            }
        }

        else
        {
            if (CanAutoAttack())
            {
                Attack();
            }
        }
    }

    private bool CanAutoAttack()
    {
        float interval = 1f / _status.AtkSpeed;

        if (Time.time - _lastAtkTime < interval)
        {
            return false;
        }

        return true;
    }

    protected override void Attack()
    {
        if (_rangeCheck.TargetTr == null && !_autoAttack)
        {
            return;
        }

        _state.IsAttacking = true;

        base.Attack();
    }

    #region 애니메이션 이벤트 함수
    public override void OnHitTarget()
    {
        if (_rangeCheck.TargetTr == null && !_autoAttack)
        {
            return;
        }

        GameObject projectile = ProjectileManager.Instance.SpawnProjectile(_projectileType);

        projectile.transform.position = _firePoint.position + _firePoint.forward * _forwardOffset;
        projectile.transform.rotation = _firePoint.rotation;

        var projectileTrigger = projectile.GetComponent<Projectile>();

        if (projectileTrigger != null)
        {
            projectileTrigger.SetProjectile(_speed, _status.AtkPower, _firePoint.forward);
        }

        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.RangedAttack, _firePoint.position, _firePoint.rotation);
    }

    public void EndAttack()
    {
        _state.IsAttacking = false;
    }
    #endregion
}