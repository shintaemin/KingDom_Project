using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSystem : MonoBehaviour, IDamageable
{
    #region 내부 변수
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

    public void TakeDamage(float amount)
    {
        float finalDmg = Mathf.Max(0, amount - _status.Armor);

        _currentHP -= finalDmg;

        if (_currentHP <= 0)
        {
            _currentHP = 0;
            _isDead = true;
            gameObject.SetActive(false);

            // 여기서 보상처리??
        }
    }
}