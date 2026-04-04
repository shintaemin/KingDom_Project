using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyDeathBroadcaster

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 적이 죽었을 때, _deadNotifyRadius 값 범위안에 있는 적들에게 알림
*/

public class EnemyDeathBroadcaster : MonoBehaviour // 추가필요 : 적 죽은 위치로 이동시켜야한다.
{
    #region 인스펙터
    [Header("범위 / 레이어 설정")]
    [SerializeField] private float _deadNotifyRadius = 5f;
    [SerializeField] private LayerMask _enemyLayer;
    #endregion

    #region 내부 변수
    private EnemyState _state;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();

        if (_state == null)
        {
            Debug.LogError("EnemyDeathBroadcaster _state 참조 실패");
            return;
        }
    }

    private void OnEnable()
    {
        if (_state != null)
        {
            _state.OnDead += DeathBroadcaster;
        }
    }

    private void OnDisable()
    {
        if (_state != null)
        {
            _state.OnDead -= DeathBroadcaster;
        }
    }

    private void DeathBroadcaster()
    {
        Collider[] nearEnemy = Physics.OverlapSphere(transform.position, _deadNotifyRadius, _enemyLayer);

        for (int i = 0; i < nearEnemy.Length; i++)
        {
            // 죽은 적(자신)은 제외
            if (nearEnemy[i].gameObject == gameObject)
            {
                continue;
            }

            var enemy = nearEnemy[i].GetComponent<EnemyState>();

            if (enemy != null)
            {
                if (enemy.GetState() == EnemyState.EState.Patrol || enemy.GetState() == EnemyState.EState.Idle)
                {
                    enemy.IsNearDead = true;
                    enemy.DeadPosition = transform.position;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _deadNotifyRadius);
    }
}