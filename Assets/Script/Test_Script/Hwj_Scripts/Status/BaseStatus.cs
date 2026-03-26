using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus : MonoBehaviour
{
    #region 인스펙터
    [Header("능력치 설정")]
    [SerializeField] protected float _maxHP = 500f;
    [SerializeField] protected float _atkPower = 100f;
    [SerializeField] protected float _atkRange = 1f;
    [SerializeField] protected float _atkSpeed = 1f;
    [SerializeField] protected float _moveSpeed = 3f;
    [SerializeField] protected float _armor = 0f;
    #endregion

    #region 프로퍼티
    public float MaxHP => _maxHP;
    public float AtkPower => _atkPower;
    public float AtkRange => _atkRange;
    public float AtkSpeed => _atkSpeed;
    public float MoveSpeed => _moveSpeed;
    public float Armor => _armor;
    #endregion
}