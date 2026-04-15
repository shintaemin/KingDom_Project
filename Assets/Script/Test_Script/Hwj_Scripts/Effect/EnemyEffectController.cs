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
    [SerializeField] private Vector3 _yOffset = new Vector3(0, 2, 0);
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
            _enemyState.OnDead += EnemyDead;
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
            _enemyState.OnDead -= EnemyDead;
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
                EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.SurprisedMark, transform.position + _yOffset, transform.rotation, transform);
                break;

            case EnemyState.EState.ChaseFail:
                EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.QuestionMark, transform.position + _yOffset, transform.rotation, transform);
                break;

            case EnemyState.EState.BossRoar:
                EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.BossRoar, transform.position, transform.rotation);
                break;

            case EnemyState.EState.BossJump:
                EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.BossJump, transform.position, transform.rotation);
                break;
        }
    }

    private void EnemyDead()
    {
        // 적 죽었을때 이펙트들 (다이아 쏟아지기..?)
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadBlood, transform.position, transform.rotation);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadCircle, transform.position, transform.rotation);
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
                SoundManager.Instance.SFXPlay(ESfxType.Fighter_Normal_Attack, true);
                break;

            case EEnemyType.Boss:
                SoundManager.Instance.SFXPlay(ESfxType.Big_Monster_Attack, true);
                break;
        }
    }

    private void BackAttackDead(bool backAtkDead)
    {
        if (backAtkDead)
        {
            // 레그돌 + 뼈
            EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.PlayerDamaged, transform.position, transform.rotation);
        }

        else
        {
            // 뼈
        }
    }
}
