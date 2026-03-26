using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : BaseStatus
{
    #region 외부 호출 함수
    public void SetStatus(float hp, float atkPower, float atkRange, float atkSpeed, float moveSpeed, float armor)
    {
        _maxHP = hp;
        _atkPower = atkPower;
        _atkRange = atkRange;
        _atkSpeed = atkSpeed;
        _moveSpeed = moveSpeed;
        _armor = armor;
    }
    #endregion
}