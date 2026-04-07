using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileFactory;

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

    [Header("수명 설정")]
    [SerializeField] private float _lifeTime = 3f;
    #endregion

    #region 내부 변수
    private bool _onHit = false;
    private float _damage = 0f;
    #endregion

    private void Reset()
    {
        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_onlyOnce && _onHit)
        {
            return;
        }

        if (other.CompareTag(_playerTag))
        {
            _onHit = true;

            var playerHp = other.GetComponent<HpSystem>();

            if (playerHp != null)
            {
                playerHp.TakeDamage(_damage, transform.position);
            }

            // 디스폰 요청
        }

        else if (((1 << other.gameObject.layer) & _notTerrainLayer) != 0)
        {
            // 디스폰 요청
        }
    }

    private IEnumerator CoLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        //ProjectileFactory.Instance.DespawnProjectile(, this.gameObject);
    }

    #region 외부 호출 함수
    public void SetProjectile(float damage)
    {
        _damage = damage;
        _onHit = false;
    }
    #endregion
}