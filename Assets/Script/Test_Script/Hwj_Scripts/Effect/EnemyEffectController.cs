using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileManager;

/*
    �� EnemyEffectController

    �� �ۼ��� : Ȳ����

    �� ��� : 
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

    #region �ν�����
    [SerializeField] private EEnemyType _enemyType;
    [SerializeField] private Vector3 _yOffset = new Vector3(0, 2, 0);

    [Header("���׵� ���� ���� (���)")]
    [SerializeField] private float _angleRangeX = 30f;
    [SerializeField] private float _angleY = 60f;
    [SerializeField] private float _forceAmount = 10f;
    [SerializeField] private float _rotateAmount = 10f;

    [Header("���׵� ���� ����")]
    [SerializeField] private float _delayTime = 3f;
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private float _duration = 2f;
    #endregion

    #region ���� ����
    private EnemyState _enemyState;
    private HpSystem _hpSystem;
    private BaseCombat _combat;
    private CInGameCanvas _uiCanvas;
    #endregion

    void Awake()
    {
        if (_enemyType == EEnemyType.None)
        {
            Debug.LogError("EnemyEffectController �� Ÿ�� ���� �ʿ�");
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

        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged += Damaged;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnBlocked += Blocked;
        }

        if (_hpSystem != null)
        {
            _hpSystem.IsBackAttackDead += BackAttackDead;
        }

        if (_combat != null)
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

    private void Start()
    {
        if (_uiCanvas == null)
        {
            _uiCanvas = FindFirstObjectByType<CInGameCanvas>();
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
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadCircle, transform.position, transform.rotation);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.Bone, transform.position, transform.rotation);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.GemExplosion, transform.position, transform.rotation);

        if (_uiCanvas != null)
        {
            CInGameCanvas.WorldToUI(transform.position, out Vector3 upPos);
            _uiCanvas.SpwanIcon(CInstancePanel.EIconType.Skull, upPos);
        }

        if (_enemyType == EEnemyType.Zombie)
        {
            EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadZombie, transform.position, transform.rotation);
        }
        else
        {
            EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.DeadBlood, transform.position, transform.rotation);
        }
    }

    private void Damaged()
    {
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.PlayerHit, transform.position + Vector3.up, transform.rotation);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.OnHitBlood, transform.position, transform.rotation);

    }

    private void Blocked()
    {
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.Block, transform.position + _yOffset, transform.rotation, transform);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.Block2, transform.position + Vector3.up, transform.rotation, transform);
        EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.HitBlock, transform.position + Vector3.up, transform.rotation, transform);
    }

    private void Attacked()
    {
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
            EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.PlayerDamaged, transform.position, transform.rotation);
            EffectManager.Instance.SpawnEffect(EffectManager.EEffectType.BackAttack, transform.position, transform.rotation);

            // ���׵�
            switch (_enemyType)
            {
                case EEnemyType.Sword:
                case EEnemyType.Bow:
                    GameObject enemyragdoll = ProjectileManager.Instance.SpawnProjectile(ProjectileManager.EProjectileType.EnemyRagdoll);

                    if (enemyragdoll != null)
                    {
                        ShootRagdoll(enemyragdoll);
                    }

                    ProjectileManager.Instance.StartCoroutine(CoFallRagdoll(enemyragdoll, EProjectileType.EnemyRagdoll));
                    break;

                case EEnemyType.Boss:
                    GameObject bossragdoll = ProjectileManager.Instance.SpawnProjectile(ProjectileManager.EProjectileType.BossRagdoll);

                    if (bossragdoll != null)
                    {
                        ShootRagdoll(bossragdoll);
                    }

                    ProjectileManager.Instance.StartCoroutine(CoFallRagdoll(bossragdoll, EProjectileType.BossRagdoll));
                    break;
            }
        }
    }

    private void ShootRagdoll(GameObject go)
    {
        go.transform.SetPositionAndRotation(transform.position, transform.rotation);

        Rigidbody[] rbs = go.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            float randomX = Random.Range(-_angleRangeX, _angleRangeX);

            Quaternion rot = Quaternion.Euler(-_angleY, randomX, 0);

            Vector3 finalDir = transform.rotation * rot * Vector3.forward;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            float randForce = Random.Range(_forceAmount * 0.9f, _forceAmount * 1.1f);
            float randRotate = Random.Range(_rotateAmount * 0.9f, _rotateAmount * 1.1f);

            rb.AddForce(finalDir * randForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * randRotate, ForceMode.Impulse);
        }
    }

    private IEnumerator CoFallRagdoll(GameObject go, EProjectileType type)
    {
        yield return new WaitForSeconds(_delayTime);

        Rigidbody[] rbs = go.GetComponentsInChildren<Rigidbody>();
        Collider[] cols = go.GetComponentsInChildren<Collider>();

        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }

        foreach (var col in cols)
        {
            col.enabled = false;
        }

        float timer = 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;
            go.transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
            yield return null;
        }

        ProjectileManager.Instance.DespawnProjectile(type, go);
    }
}