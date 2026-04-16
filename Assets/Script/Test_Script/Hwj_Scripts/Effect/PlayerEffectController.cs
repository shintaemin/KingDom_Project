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
    #region 인스펙터
    [Header("적 클릭시 파티클 쿨타임")]
    [SerializeField] private float _clickCooldown = 1f;

    [Header("히트 파티클 보정")]
    [SerializeField] private Vector3 _hitOffset = new Vector3(0, 1, 0);
    #endregion

    #region 내부 변수
    private PlayerState _playerState;
    private PlayerMover _playerMover;
    private PlayerPathRecorder _pathRecorder;
    private HpSystem _hpSystem;
    private BaseCombat _combat;
    private PlayerCombat _playerCombat;
    private CInGameCanvas _uiCanvas;
    private float _lastClick;
    #endregion

    void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _playerMover = GetComponent<PlayerMover>();
        _pathRecorder = GetComponent<PlayerPathRecorder>();
        _hpSystem = GetComponent<HpSystem>();
        _combat = GetComponent<BaseCombat>();
        _playerCombat = GetComponent<PlayerCombat>();
        _uiCanvas = FindFirstObjectByType<CInGameCanvas>();
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

        if (_playerCombat != null)
        {
            _playerCombat.OnHit += HitTarget;
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

        if (_playerCombat != null)
        {
            _playerCombat.OnHit -= HitTarget;
        }
    }

    private void PlayerDead()
    {
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadBlood, transform.position, transform.rotation);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadCircle, transform.position, transform.rotation);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.PlayerDamaged, transform.position, transform.rotation);
    }

    private void ClickEnemy()
    {
        if (Time.time - _lastClick < _clickCooldown)
        {
            return;
        }

        _lastClick = Time.time;

        Transform enemy = _pathRecorder.GetEnemy();

        if (enemy != null)
        {
            EffectManager.Instance.SpawnEffect
                (
                EffectManager.EEffectType.ClickEnemy,
                enemy.position,
                enemy.rotation,
                enemy
                );
        }
    }

    private void Damaged()
    {
        // 플레이어가 데미지를 입었을 때 이펙트 (피격 이펙트)
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.PlayerDamaged, transform.position, transform.rotation);
        _uiCanvas.CallFullscreenImpact(Color.red);
    }

    private void Attacked()
    {
        // 플레이어가 공격 시 이펙트 (공격 이펙트)
    }

    private void HitTarget()
    {
        Vector3 spawnPos = transform.position + (transform.forward * _hitOffset.z) + (transform.up * _hitOffset.y) + (transform.right * _hitOffset.x);
        
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.PlayerHit, spawnPos, transform.rotation);
    }
}
