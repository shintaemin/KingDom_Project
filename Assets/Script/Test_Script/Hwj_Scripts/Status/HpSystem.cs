using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ HpSystem

    ㆍ 작성자 : 황원준

    ㆍ 기능 : IDamageable 인터페이스를 구현하여, 방어력이 적용된 최종 데미지 계산 및 객체의 사망 상태 관리
*/

public class HpSystem : MonoBehaviour, IDamageable
{
    #region 인스펙터
    [Header("방패병 설정")]
    [SerializeField] private bool _isShielded = false;
    #endregion

    #region 내부 변수
    public event System.Action OnDamaged;
    public event System.Action OnBlocked;
    public event System.Action<bool> IsBackAttackDead;
    private BaseStatus _status;
    private float _currentHP;
    private bool _isDead = false;
    #endregion

    #region 프로퍼티
    public bool IsDead => _isDead;
    #endregion

    private void Awake()
    {
        _status = GetComponent<BaseStatus>();

        if (_status == null)
        {
            Debug.LogError("HpSystem _status 참조 실패");
            return;
        }
    }

    void Start()
    {
        if (_status != null)
        {
            _currentHP = _status.MaxHP;
        }
    }

    public void TakeDamage(float amount, Vector3 attackerPosition, bool isBackAttackDead = false)
    {
        if (_isDead)
        {
            return;
        }

        if (_isShielded)
        {
            Vector3 attackerPos = attackerPosition;
            Vector3 pos = transform.position;

            attackerPos.y = 0;
            pos.y = 0;

            Vector3 direction = (attackerPos - pos).normalized;

            float dot = Vector3.Dot(transform.forward, direction);

            if (dot > 0.5)
            {
                Debug.Log("방패로 막았음");
                OnBlocked?.Invoke();
                return;
            }
        }

        float finalDmg = Mathf.Max(0, amount - _status.Armor);

        _currentHP -= finalDmg;

        if (_currentHP <= 0)
        {
            _currentHP = 0;
            _isDead = true;
            IsBackAttackDead?.Invoke(isBackAttackDead);
        }

        else
        {
            OnDamaged?.Invoke();
        }
    }

    #region 외부 호출 함수
    public float GetCurrentHP()
    {
        return _currentHP;
    }

    public float GetMaxHP()
    {
        return _status.MaxHP;
    }

    public float GetHPPercentage()
    {
        return _currentHP / _status.MaxHP;
    }
    #endregion
}