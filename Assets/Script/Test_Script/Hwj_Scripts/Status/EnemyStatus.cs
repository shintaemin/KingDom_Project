using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyStatus

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 스테이지가 진행됨에 따라 적의 체력과 공격력, 보상이 일정 비율로 자동 상승하는 시스템
*/

public class EnemyStatus : BaseStatus
{
	#region 인스펙터
	[Header("스테이지별 체력 공격력 비율")]
	[SerializeField] private float _hpRatio = 1.1f;
	[SerializeField] private float _atkRatio = 1.1f;

    [Header("적 처치시 재화 양")]
	[SerializeField] private float _diamond = 200f;
    #endregion

    #region 내부 변수
    private float _baseMaxHP;
    private float _baseAtkPower;
    #endregion

    #region 프로퍼티
    public float Diamond => _diamond;
    #endregion

    private void Awake()
    {
        _baseMaxHP = _maxHP;
        _baseAtkPower = _atkPower;
    }

    #region 외부 호출 함수
    public void SetStatus(int currentStage)
	{
		int increase = currentStage - 1;

        _maxHP = _baseMaxHP * Mathf.Pow(_hpRatio, increase);
        _atkPower = _baseAtkPower * Mathf.Pow(_atkRatio, increase);
    }
    #endregion
}