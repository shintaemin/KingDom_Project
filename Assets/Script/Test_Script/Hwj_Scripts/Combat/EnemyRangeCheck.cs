using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyRangeCheck

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 태그를 기반으로 플레이어를 타겟으로 설정하고 사거리 진입 시 적의 상태 갱신
*/

public class EnemyRangeCheck : BaseRangeCheck
{
    #region 인스펙터
    [Header("플레이어 태그")]
    [SerializeField] private string _playerTag = "Player";
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
            Debug.LogError("EnemyRangeCheck _state 참조 실패");
            return;
        }

        var player = GameObject.FindWithTag(_playerTag);

        if (player != null)
        {
            _targetTr = player.transform;

            if (_targetTr == null)
            {
                Debug.LogError("EnemyDetectRange _playerTr 참조 실패 (태그 설정 필요)");
                return;
            }
        }
    }
    protected override void Update()
    {
        base.Update();

        if (_state != null)
        {
            _state.IsAtkRange = _isAtkRange;
        }
    }

    // 예외 상황 처리 (혹시나 플레이어를 못찾았을 경우)
    protected override void UpdateTarget()
    {
        if (_targetTr == null)
        {
            var player = GameObject.FindWithTag(_playerTag);

            if (player != null)
            {
                _targetTr = player.transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_status == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _status.AtkRange);
    }
}