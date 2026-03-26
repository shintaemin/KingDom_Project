using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeCheck : BaseRangeCheck
{
    #region 인스펙터
    [Header("레이어 설정")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _findInterval = 0.2f;
    [SerializeField] private float _rangeOffset = 2f;
    #endregion

    #region 내부 변수
    private float _nextFindTime = 0.0f;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateTarget()
    {
        if (Time.time < _nextFindTime)
        {
            return;
        }

        _nextFindTime = Time.time + _findInterval;

        Collider[] Enemies = Physics.OverlapSphere(transform.position, _status.AtkRange * _rangeOffset, _enemyLayer);

        Transform nearEnemy = null;

        float bestSqr = float.MaxValue;

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == null)
            {
                continue;
            }

            Transform enemy = Enemies[i].transform;

            Vector3 direction = enemy.position - transform.position;
            float sqr = direction.sqrMagnitude;

            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                nearEnemy = enemy;
            }
        }

        _targetTr = nearEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        if (_status == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _status.AtkRange * _rangeOffset);
    }
}