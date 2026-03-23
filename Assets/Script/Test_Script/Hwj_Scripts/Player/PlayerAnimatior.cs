using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerAnimatior

    ㆍ 작성자 : 황원준

    ㆍ 기능 : PlayerState의 OnStateChanged 이벤트를 구독하여 플레이어 애니메이션 제어
*/

public class PlayerAnimatior : MonoBehaviour // IK 쓰기
{
    #region 인스펙터
    [SerializeField] private string _paramMove = "bMove";
    [SerializeField] private string _paramDead = "tDead";
    #endregion

    #region 내부 변수
    private PlayerState _state;
    private Animator _anim;
    private int _hashMove;
    private int _hashDead;
    #endregion

    private void Awake()
    {
        _state = GetComponent<PlayerState>();
        _anim = GetComponent<Animator>();

        if (_state == null || _anim == null)
        {
            Debug.LogError("PlayerAnimatior _state _anim 참조 실패");
            return;
        }

        _hashMove = Animator.StringToHash(_paramMove);
        _hashDead = Animator.StringToHash(_paramDead);
    }

    private void OnEnable()
    {
        if (_state != null)
        {
            _state.OnStateChanged += StateChanged;
        }
    }

    private void OnDisable()
    {
        if (_state != null)
        {
            _state.OnStateChanged -= StateChanged;
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
}