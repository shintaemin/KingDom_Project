using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 플레이어 데이터 SO
/*
 ▶ 할일
  - 플레이어 데이터를 관리
*/
#endregion


[CreateAssetMenu(menuName = "CreateSO/Data_SO/Player_Data_SO", fileName = "Player_Data_SO")]
public class Player_Data_SO : ScriptableObject
{
    #region 인스펙터
    [Header("데이터")]
    [SerializeField] private float _power;
    [SerializeField] private float _health;
    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _startPower;
    [SerializeField] private float _startHealth;
    [SerializeField] private float _startSpeed;

    [Header("스테이지 , 챕터 데이터")]
    [SerializeField] private int _chapter = 1;
    [SerializeField] private int _stage = 1;
    #endregion

    #region 외부 호출 함수
    public float GetPower => _power;
    public float GetHealth => _health;
    public float GetMoveSpeed => _moveSpeed;
    public int GetChapter => _chapter;
    public int GetStage => _stage;

    public void AddPower(float power, bool percent = true)
    {
        if (power == 0)
        {
            return;
        }

        power = percent ? (_power / 100) * power : power;

        _power += power;
    }

    public void AddHealth(float health, bool percent = true)
    {
        if (health == 0)
        {
            return;
        }

        health = percent ? (_health / 100) * health : health;

        _health += health;
    }

    public void AddSpeed(float speed, bool percent = true)
    {
        if (speed == 0)
        {
            return;
        }

        speed = percent ? (_moveSpeed / 100) * speed : speed;

        _moveSpeed += speed;
    }

    public void AddChapter()
    {
        _chapter++;
    }

    public void AddState()
    {
        _stage++;
    }

    public void ResetData(bool stageClear = false)
    {
        _power = _startPower;
        _health = _startHealth;
        _moveSpeed = _startSpeed;

        if (stageClear)
        {
            _chapter = 1;
            _stage = 1;
        }
    }
    #endregion
}
