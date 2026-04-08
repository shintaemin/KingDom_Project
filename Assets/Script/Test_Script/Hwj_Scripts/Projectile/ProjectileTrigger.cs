using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ ProjectileTrigger

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 투사체의 충돌 및 대미지 처리 담당 / 생성 시 SetProjectile 함수를 통해 대미지 설정
*/

public class ProjectileTrigger : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private LayerMask _notTerrainLayer;
    [SerializeField] private bool _onlyOnce = true;

    [Header("타입 설정")]
    [SerializeField] private ProjectileFactory.ProjectileType _projectileType;

    [Header("수명 설정")]
    [SerializeField] private float _lifeTime = 3f;
    #endregion

    #region 내부 변수
    private bool _onHit = false;
    private float _damage = 0f;
    private Rigidbody _rb;
    private Coroutine _lifeTimeRoutine;
    #endregion

    private void Reset()
    {
        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("ProjectileTrigger _rb 참조 실패");
            return;
        }

        if (_projectileType == ProjectileFactory.ProjectileType.None)
        {
            Debug.LogError("ProjectileTrigger _projectileType 인스펙터 확인");
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_onlyOnce && _onHit)
        {
            return;
        }

        _onHit = true;

        if (other.CompareTag(_playerTag))
        {
            var playerHp = other.GetComponent<HpSystem>();

            if (playerHp != null)
            {
                playerHp.TakeDamage(_damage, transform.position);

                ReturnToPool();
            }
        }

        else if (((1 << other.gameObject.layer) & _notTerrainLayer) != 0)
        {
            if (_projectileType == ProjectileFactory.ProjectileType.Arrow)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;

                //_rb.isKinematic = true; 테스트 후에 느낌이 없다면 적용
            }

            else
            {
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (_lifeTimeRoutine != null)
        {
            StopCoroutine(_lifeTimeRoutine);
        }

        _lifeTimeRoutine = null;

        ProjectileFactory.Instance.DespawnProjectile(_projectileType, this.gameObject);
    }

    private IEnumerator CoLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);

        ReturnToPool();
    }

    #region 외부 호출 함수
    public void SetProjectile(float speed, float damage, Vector3 dir)
    {
        _onHit = false;
        _damage = damage;

        //_rb.isKinematic = false; 테스트 후에 느낌이 없다면 적용

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        _rb.AddForce(speed * dir.normalized, ForceMode.VelocityChange);

        if (_lifeTimeRoutine != null)
        {
            StopCoroutine(_lifeTimeRoutine);
        }

        _lifeTimeRoutine = StartCoroutine(CoLifeTime());
    }
    #endregion
}