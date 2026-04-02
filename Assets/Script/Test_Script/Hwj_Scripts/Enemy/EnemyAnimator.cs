using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
    ㆍ EnemyAnimator

    ㆍ 작성자 : 황원준

    ㆍ 기능 : EnemyState의 OnStateChanged 이벤트를 구독하여 플레이어 애니메이션 제어
*/

public class EnemyAnimator : MonoBehaviour // 속도값 받아올 수 있을때 bool move -> float move 로 변경
{
    #region 인스펙터
    [SerializeField] private string _paramWalk = "bWalk";
    [SerializeField] private string _paramRun = "bRun";
    //[SerializeField] private string _paramDead = "tDead";
    [SerializeField] private string _paramAttack = "tAttack";
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private EnemyCombat _combat;
    private Animator _anim;
    private int _hashRun;
    //private int _hashDead;
    private int _hashAttack;
    private int _hashWalk;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();
        _anim = GetComponent<Animator>();
        _combat = GetComponent<EnemyCombat>();

        if (_state == null || _anim == null || _combat == null)
        {
            Debug.LogError("EnemyAnimator _state _anim _combat 참조 실패");
            return;
        }

        _hashRun = Animator.StringToHash(_paramRun);
        //_hashDead = Animator.StringToHash(_paramDead);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashWalk = Animator.StringToHash(_paramWalk);
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

    private void StateChanged(EnemyState.EState state)
    {
        switch (state)
        {
            case EnemyState.EState.Idle:
                _anim.SetBool(_hashWalk, false);
                break;

            case EnemyState.EState.Patrol:
                _anim.SetBool(_hashWalk, true);
                break;

            case EnemyState.EState.Detect:
                // 딱히 감지 애니메이션은 없는듯 (! 이미지 띄우고 플레이어를 처다보는 정도?)
                _anim.SetBool(_hashWalk, false);
                break;

            case EnemyState.EState.Chase:
                _anim.SetBool(_hashRun, true);
                break;

            case EnemyState.EState.ChaseFail:
                _anim.SetBool(_hashRun, false);
                // (? 이미지 띄우고 두리번..?)
                break;

            case EnemyState.EState.Attack:
                _anim.SetBool(_hashRun, false);
                _anim.SetBool(_hashWalk, false);
                break;

            case EnemyState.EState.Dead:
                //_anim.SetTrigger(_hashDead);
                break;
        }
    }

    private void Attacked()
    {
        _anim.SetTrigger(_hashAttack);
    }
}