using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyDetectRange

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 적의 정면 기준 부채꼴 범위 내 플레이어 감지
*/

public class EnemyDetectRange : MonoBehaviour
{
    #region 인스펙터
    [Header("감지(시야) 범위 설정")]
    [SerializeField] private float _detectDistance = 10f;
    [SerializeField] private float _detectAngle = 60f;

    [Header("장애물(벽) 레이어 설정")]
    [SerializeField] private LayerMask _obstacleLayer;

    [Header("플레이어 태그")]
    [SerializeField] private string _playerTag = "Player";
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private Transform _playerTr;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();

        if (_state == null)
        {
            Debug.LogError("EnemyDetectRange _state 참조 실패");
            return;
        }

        var player = GameObject.FindWithTag(_playerTag);

        if (player != null)
        {
            _playerTr = player.transform;

            if (_playerTr == null)
            {
                Debug.LogError("EnemyDetectRange _playerTr 참조 실패 (태그 설정 필요)");
                return;
            }
        }
    }

    void Update()
    {
        CheckDetect();
    }

    private void CheckDetect()
    {
        if (_playerTr == null)
        {
            return;
        }

        _state.IsDetected = false;

        float distance = Vector3.Distance(transform.position, _playerTr.position);

        if (distance > _detectDistance)
        {
            return;
        }

        Vector3 direction = (_playerTr.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, direction);

        if (angle <= _detectAngle * 0.5f) // 레이캐스트 y보정값이 필요할수도 있음.
        {
            if (!Physics.Raycast(transform.position, direction, distance, _obstacleLayer))
            {
                _state.IsDetected = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * _detectDistance);

        Vector3 leftLine = Quaternion.Euler(0, -_detectAngle * 0.5f, 0) * transform.forward;
        Vector3 rightLine = Quaternion.Euler(0, _detectAngle * 0.5f, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + leftLine * _detectDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightLine * _detectDistance);
    }
}