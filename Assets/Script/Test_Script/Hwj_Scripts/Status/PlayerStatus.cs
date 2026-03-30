using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerStatus

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 플레이어의 능력치를 외부에서 받아와 설정할 수 있는 Set함수를 통해 데이터 연동
*/

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