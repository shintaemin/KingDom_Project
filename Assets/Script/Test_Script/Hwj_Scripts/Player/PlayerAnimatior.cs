using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerAnimatior

    ㆍ 작성자 : 황원준

    ㆍ 기능 : PlayerState의 OnStateChanged 이벤트를 구독하여 플레이어 애니메이션 제어
*/

public class PlayerAnimatior : MonoBehaviour // IK 쓰기
{
    #region 인스펙터
    [SerializeField] private string _paramSpeed = "fSpeed";
    [SerializeField] private string _paramDead = "tDead";
    [SerializeField] private string _paramAtk = "tAttack";
    [SerializeField] private string _paramCombo = "iComboIndex";
    #endregion

    #region 내부 변수
    private PlayerState _state;
    private PlayerCombat _combat;
    private Animator _anim;
    private NavMeshAgent _nav;
    private int _hashSpeed;
    private int _hashDead;
    private int _hashAtk;
    private int _hashCombo;
    private int _comboIndex = 0;
    #endregion

    private void Awake()
    {
        _state = GetComponent<PlayerState>();
        _anim = GetComponent<Animator>();
        _combat = GetComponent<PlayerCombat>();
        _nav = GetComponent<NavMeshAgent>();

        if (_state == null || _anim == null || _combat == null || _nav == null)
        {
            Debug.LogError("PlayerAnimatior _state _anim _combat _nav 참조 실패");
            return;
        }

        _hashSpeed = Animator.StringToHash(_paramSpeed);
        _hashDead = Animator.StringToHash(_paramDead);
        _hashAtk = Animator.StringToHash(_paramAtk);
        _hashCombo = Animator.StringToHash(_paramCombo);
    }

    private void OnEnable()
    {
        if (_state != null)
        {
            _state.OnStateChanged += StateChanged;
        }

        if (_combat != null)
        {
            _combat.OnAttacked += Attacked;
        }
    }

    private void OnDisable()
    {
        if (_state != null)
        {
            _state.OnStateChanged -= StateChanged;
        }

        if (_combat != null)
        {
            _combat.OnAttacked -= Attacked;
        }
    }

    void Update()
    {
        float CurrentSpeed = _nav.velocity.magnitude / _nav.speed;

        _anim.SetFloat(_hashSpeed, CurrentSpeed);
    }

    private void StateChanged(PlayerState.EState state)
    {
        switch (state)
        {
            case PlayerState.EState.Idle:
                
                break;

            case PlayerState.EState.Moving:
                
                break;

            case PlayerState.EState.Dead:
                _anim.SetTrigger(_hashDead);
                break;
        }
    }

    private void Attacked()
    {
        _anim.SetInteger(_hashCombo, _comboIndex);

        _anim.SetTrigger(_hashAtk);

        _comboIndex = (_comboIndex == 0) ? 1 : 0;
    }
}