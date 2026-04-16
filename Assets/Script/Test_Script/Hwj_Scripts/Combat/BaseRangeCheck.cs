using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ BaseRangeCheck

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 공격 사거리를 체크하는 기본 기반 클래스 → 타겟과 거리를 계산하여 공격 사거리 판단
*/

public abstract class BaseRangeCheck : MonoBehaviour
{
    #region 인스펙터
    [Header("공격시 시야 체크 설정")]
    [SerializeField] protected LayerMask _notTerrain;
    [SerializeField] protected float _yOffset = 0.5f;
    #endregion

    #region 내부 변수
    protected BaseStatus _status;
    protected Transform _targetTr;
    protected bool _isAtkRange = false;
    #endregion

    #region 프로퍼티
    public bool IsAtkRange => _isAtkRange;
    public Transform TargetTr => _targetTr;
    #endregion

    protected virtual void Awake()
    {
        _status = GetComponent<BaseStatus>();

        if (_status == null)
        {
            Debug.LogError("BaseRangeCheck _status 참조 실패");
            return;
        }
    }

    protected virtual void Update()
    {
        UpdateTarget();
        DistanceCheck();
    }

    protected void DistanceCheck()
    {
        if (_targetTr == null)
        {
            _isAtkRange = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, _targetTr.position);

        if (distance <= _status.AtkRange && IsWallFront())
        {
            _isAtkRange = true;
        }

        else
        {
            _isAtkRange = false;
        }
    }

    protected bool IsWallFront()
    {
        if (_targetTr == null)
        {
            return false;
        }

        Vector3 startPos = transform.position + Vector3.up * _yOffset;
        Vector3 targetPos = _targetTr.position + Vector3.up * _yOffset;
        Vector3 direction = (targetPos - startPos).normalized;

        float distance = Vector3.Distance(startPos, targetPos);

        if (Physics.Raycast(startPos, direction, out RaycastHit hit, distance, _notTerrain))
        {
            return false;
        }

        return true;
    }

    protected abstract void UpdateTarget();
}