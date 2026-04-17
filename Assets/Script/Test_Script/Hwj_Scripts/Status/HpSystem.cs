using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
    ㆍ HpSystem

    ㆍ 작성자 : 황원준

    ㆍ 기능 : IDamageable 인터페이스를 구현하여, 방어력이 적용된 최종 데미지 계산 및 객체의 사망 상태 관리
*/

public class HpSystem : MonoBehaviour, IDamageable, IHPBar
{
    #region 인스펙터
    [Header("방패병 설정")]
    [SerializeField] private bool _isShielded = false;
    [SerializeField] private CHPBar _hpbar;
    #endregion

    #region 내부 변수
    public event System.Action OnDamaged;
    public event System.Action OnBlocked;
    public event System.Action OnDead;
    public event System.Action<bool> IsBackAttackDead;
    public event Action<float> OnHealthChanged;
    public event Action<Vector3> OnPositionChanged;

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

        if (_hpbar == null)
        {
            if (TryGetComponent<CHPBar>(out _hpbar))
            {
                Debug.Log($"[HpSystem] : _hpbar 스크립트 캐싱 성공");
                Transform uiSpawnTr = CInGameCanvas.GetSpawnRootTransform();
                _hpbar.InitSpawnPos(uiSpawnTr);
                Color col = gameObject.CompareTag("Player") ? Color.green : Color.red;
                _hpbar.SetFillColor(col);
                OnHealthChanged?.Invoke(_currentHP);
            }
            else
            {
                Debug.LogWarning($"[HpSystem] : {this.gameObject.name}_hpBar 스크립트 캐싱 실패");
            }
        }
        else
        {
            Debug.LogWarning("[HpSystem] : _hpbar 가 이미 있음");
        }
    }

    private void Update()
    {
        OnPositionChanged?.Invoke(gameObject.transform.position);
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
                OnBlocked?.Invoke();
                return;
            }
            else
            {
                OnDamaged?.Invoke();
            }
        }

        float finalDmg = Mathf.Max(0, amount - _status.Armor);

        _currentHP -= finalDmg;

        if (_currentHP <= 0)
        {
            _currentHP = 0;
            _isDead = true;
            OnDead?.Invoke();
            OnDamaged?.Invoke();
            IsBackAttackDead?.Invoke(isBackAttackDead);
        }

        else
        {
            OnDamaged?.Invoke();
            OnHealthChanged?.Invoke(GetHPPercentage());
        }
    }

    #region 외부 호출 함수
    public float GetCurrentHP()
    {
        OnHealthChanged?.Invoke(_currentHP);
        return _currentHP;
    }

    public float GetMaxHP()
    {
        OnHealthChanged?.Invoke(_currentHP);
        return _status.MaxHP;
    }

    public float GetHPPercentage()
    {
        return _currentHP / _status.MaxHP;
    }
    #endregion
}