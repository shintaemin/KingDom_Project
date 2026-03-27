using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerAnimatior

    ㆍ 작성자 : 황원준

    ㆍ 기능 : PlayerState의 OnStateChanged 이벤트를 구독하여 플레이어 애니메이션 제어
*/

public class PlayerAnimatior : MonoBehaviour // IK 쓰기, 속도값 받아올 수 있을때 bool move -> float move 로 변경
{
    #region 인스펙터
    [SerializeField] private string _paramMove = "bMove";
    [SerializeField] private string _paramDead = "tDead";
    [SerializeField] private string _paramAtk = "tAttack";
    #endregion

    #region 내부 변수
    private PlayerState _state;
    private PlayerCombat _combat;
    private Animator _anim;
    private int _hashMove;
    private int _hashDead;
    private int _hashAtk;
    #endregion

    private void Awake()
    {
        _state = GetComponent<PlayerState>();
        _anim = GetComponent<Animator>();
        _combat = GetComponent<PlayerCombat>();

        if (_state == null || _anim == null || _combat == null)
        {
            Debug.LogError("PlayerAnimatior _state _anim _combat 참조 실패");
            return;
        }

        _hashMove = Animator.StringToHash(_paramMove);
        _hashDead = Animator.StringToHash(_paramDead);
        _hashAtk = Animator.StringToHash(_paramAtk);
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

    private void StateChanged(PlayerState.EState state)
    {
        switch (state)
        {
            case PlayerState.EState.Idle:
                _anim.SetBool(_hashMove, false);
                break;

            case PlayerState.EState.Moving:
                _anim.SetBool(_hashMove, true);
                break;

            case PlayerState.EState.Dead:
                _anim.SetTrigger(_hashDead);
                break;
        }
    }

    private void Attacked()
    {
        // 공격 애니메이션
        _anim.SetTrigger(_hashAtk);
    }
}