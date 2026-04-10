using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ EnemyAnimator

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 적의 이동 속도 및 공격 상태 애니메이터 파라미터와 동기화
*/

public class EnemyAnimator : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _paramSpeed = "fSpeed";
    [SerializeField] private string _paramAttack = "tAttack";
    [SerializeField] private string _paramOnHit = "tOnHit";
    [SerializeField] private string _paramOnHitIndex = "iOnHitIndex";

    [Header("18 스테이지 전용")]
    [SerializeField] private bool _autoAttack = false;
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private BaseCombat _combat;
    private HpSystem _hpSystem;
    private Animator _anim;
    private NavMeshAgent _nav;
    private int _hashSpeed;
    private int _hashAttack;
    private int _hashOnHit;
    private int _hashOnHitIndex;
    private int _onHitIndex = 0;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();
        _combat = GetComponent<BaseCombat>();
        _hpSystem = GetComponent<HpSystem>();
        _anim = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();

        if (_state == null || _anim == null || _hpSystem  == null)
        {
            Debug.LogError("EnemyAnimator _state _anim _hpSystem 참조 실패");
            return;
        }

        _hashSpeed = Animator.StringToHash(_paramSpeed);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashOnHit = Animator.StringToHash(_paramOnHit);
        _hashOnHitIndex = Animator.StringToHash(_paramOnHitIndex);
    }

    private void OnEnable()
    {
        if (_combat != null)
        {
            _combat.OnAttacked += Attacked;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged += Damaged;
        }
    }

    private void OnDisable()
    {
        if (_combat != null)
        {
            _combat.OnAttacked -= Attacked;
        }

        if (_hpSystem != null)
        {
            _hpSystem.OnDamaged -= Damaged;
        }
    }

    void Update()
    {
        if (_autoAttack)
        {
            return;
        }

        float CurrentSpeed = _nav.velocity.magnitude;

        _anim.SetFloat(_hashSpeed, CurrentSpeed);
    }

    private void Attacked()
    {
        _anim.SetTrigger(_hashAttack);
    }

    private void Damaged()
    {
        if (_state.IsAttacking)
        {
            return;
        }

        _anim.SetTrigger(_hashOnHit);

        _anim.SetInteger(_hashOnHitIndex, _onHitIndex);

        _onHitIndex = (_onHitIndex == 0) ? 1 : 0;
    }
}