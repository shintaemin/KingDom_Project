using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerEffectController

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class PlayerEffectController : MonoBehaviour
{
    #region 내부 변수
    private PlayerState _playerState;
    private PlayerMover _playerMover;
    private PlayerPathRecorder _pathRecorder;
    private HpSystem _hpSystem;
    private BaseCombat _combat;
    #endregion

    void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _playerMover = GetComponent<PlayerMover>();
        _pathRecorder = GetComponent<PlayerPathRecorder>();
        _hpSystem = GetComponent<HpSystem>();
        _combat = GetComponent<BaseCombat>();
    }

    private void OnEnable()
    {
        if (_playerState != null)
        {
            _playerState.OnDead += PlayerDead;
        }

        if (_pathRecorder != null)
        {
            _pathRecorder.OnClickEnemy += ClickEnemy;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged += Damaged;
        }

        if (_combat != null)
        {
            _combat.OnAttacked += Attacked;
        }
    }

    private void OnDisable()
    {
        if (_playerState != null)
        {
            _playerState.OnDead -= PlayerDead;
        }

        if (_pathRecorder != null)
        {
            _pathRecorder.OnClickEnemy -= ClickEnemy;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged -= Damaged;
        }

        if (_combat != null)
        {
            _combat.OnAttacked -= Attacked;
        }
    }

    private void PlayerDead()
    {
        // 플레이어 사망 시 이펙트 재생
    }

    private void ClickEnemy()
    {
        // 적 클릭 시 이펙트 재생 (선택 이펙트)
    }

    private void Damaged()
    {
        // 플레이어가 데미지를 입었을 때 이펙트 (피격 이펙트)
    }

    private void Attacked()
    {
        // 플레이어가 공격 시 이펙트 (공격 이펙트)
    }
}
