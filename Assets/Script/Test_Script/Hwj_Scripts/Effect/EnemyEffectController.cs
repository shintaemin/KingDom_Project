using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyEffectController

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class EnemyEffectController : MonoBehaviour
{
    private enum EEnemyType
    {
        None,
        Sword,
        Bow,
        Zombie,
        Boss
    }

    #region 인스펙터
    [SerializeField] private EEnemyType _enemyType;
    #endregion

    #region 내부 변수
    private EnemyState _enemyState;
    private HpSystem _hpSystem;
    private BaseCombat _combat;
    #endregion

    void Awake()
    {
        if (_enemyType == EEnemyType.None)
        {
            Debug.LogError("EnemyEffectController 적 타입 설정 필요");
            return;
        }

        _enemyState = GetComponent<EnemyState>();
        _hpSystem = GetComponent<HpSystem>();
        _combat = GetComponent<BaseCombat>();
    }

    private void OnEnable()
    {
        if (_enemyState != null)
        {
            _enemyState.OnStateChanged += StateChanged;
        }

        if (_enemyState != null)
        {
            _enemyState.OnDead += Dead;
        }

        if (_hpSystem != null )
        {
            _hpSystem.OnDamaged += Damaged;
        }

        if ( _hpSystem != null )
        {
            _hpSystem.OnBlocked += Blocked;
        }

        if (_hpSystem != null)
        {
            _hpSystem.IsBackAttackDead += BackAttackDead;
        }

        if (_combat != null )
        {
            _combat.OnAttacked += Attacked;
        }
    }

    private void OnDisable()
    {
        if (_enemyState != null)
        {
            _enemyState.OnStateChanged -= StateChanged;
        }

        if (_enemyState != null)
        {
            _enemyState.OnDead -= Dead;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged -= Damaged;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnBlocked -= Blocked;
        }

        if (_hpSystem != null)
        {
            _hpSystem.IsBackAttackDead -= BackAttackDead;
        }

        if (_combat != null)
        {
            _combat.OnAttacked -= Attacked;
        }
    }

    private void StateChanged(EnemyState.EState state)
    {
        switch (state)
        {
            case EnemyState.EState.Detect:
                // ! 파티클 재생
                break;

            case EnemyState.EState.ChaseFail:
                // ? 파티클 재생
                break;

            case EnemyState.EState.BossRoar:
                // 보스 포효 파티클 재생
                break;

            case EnemyState.EState.BossJump:
                // 보스 점프 파티클 재생 (있다면..? 착지까지 확인해야함)
                break;
        }
    }

    private void Dead()
    {
        // 적 죽었을때 이펙트들 (다이아 쏟아지기..?)
    }

    private void Damaged()
    {
        // 데미지 받았을 때 (피격 이펙트)
    }

    private void Blocked()
    {
        // 방패로 막았을 떄 (방어막 이펙트)
    }

    private void Attacked()
    {
        // 공격했을 때, (타입별로 확인 필요)
        switch (_enemyType)
        {
            case EEnemyType.Sword:
                // 효과음만 재생
                break;

            case EEnemyType.Bow:
                // 원형 파티클 + 효과음 (흰색)
                break;

            case EEnemyType.Zombie:
                // 원형 파티클 + 효과음 (흰색)
                break;

            case EEnemyType.Boss:
                // 확인필요
                break;
        }
    }

    private void BackAttackDead(bool backAtkDead)
    {
        if (backAtkDead)
        {
            // 레그돌 처리
        }

        else
        {
            // 일반 사망 시 이펙트 처리
        }
    }
}
