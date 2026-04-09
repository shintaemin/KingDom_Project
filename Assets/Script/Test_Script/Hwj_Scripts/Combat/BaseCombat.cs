using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ BaseCombat

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 플레이어, 적 공통 공격 로직 (사거리 체크, 공격 쿨타임 체크 등)
*/

public abstract class BaseCombat : MonoBehaviour
{
    #region 인스펙터
    [Header("공격시 회전속도 설정")]
    [SerializeField] protected float _rotSpeed = 5f;
    #endregion

    #region 내부 변수
    public event System.Action OnAttacked;
    protected BaseStatus _status;
    protected BaseRangeCheck _rangeCheck;
    protected float _lastAtkTime;
    protected NavMeshAgent _nav;
    #endregion

    protected virtual void Awake()
    {
        _status = GetComponent<BaseStatus>();
        _rangeCheck = GetComponent<BaseRangeCheck>();
        _nav = GetComponent<NavMeshAgent>();

        if (_status == null || _rangeCheck == null || _nav == null)
        {
            Debug.LogError("BaseCombat _status _rangeCheck 참조 실패");
            return;
        }
    }

    protected virtual void Update()
    {
        if (_rangeCheck.TargetTr != null && _rangeCheck.IsAtkRange)
        {
            LookTarget(_rangeCheck.TargetTr);
        }
    }

    protected void LookTarget(Transform target)
    {
        if (target == null || _nav.velocity.magnitude > 0.1f)
        {
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotSpeed);
        }
    }

    protected virtual bool CanAttack()
    {
        if (_rangeCheck == null || !_rangeCheck.IsAtkRange)
        {
            return false;
        }

        float interval = 1f / _status.AtkSpeed;

        if (Time.time - _lastAtkTime < interval)
        {
            return false;
        }

        return true;
    }

    protected virtual void Attack()
    {
        _lastAtkTime = Time.time;
        
        OnAttacked?.Invoke();
    }

    public abstract void OnHitTarget();
}