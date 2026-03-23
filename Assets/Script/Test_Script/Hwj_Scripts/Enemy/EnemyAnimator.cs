using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ EnemyAnimator

    ㆍ 작성자 : 황원준

    ㆍ 기능 : EnemyState의 OnStateChanged 이벤트를 구독하여 플레이어 애니메이션 제어
*/

public class EnemyAnimator : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _paramWalk = "bWalk";
    [SerializeField] private string _paramRun = "bRun";
    [SerializeField] private string _paramDead = "tDead";
    [SerializeField] private string _paramAttack = "tAttack";
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private Animator _anim;
    private int _hashRun;
    private int _hashDead;
    private int _hashAttack;
    private int _hashWalk;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();
        _anim = GetComponent<Animator>();

        if (_state == null || _anim == null)
        {
            Debug.LogError("EnemyAnimator _state _anim 참조 실패");
            return;
        }

        _hashRun = Animator.StringToHash(_paramRun);
        _hashDead = Animator.StringToHash(_paramDead);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashWalk = Animator.StringToHash(_paramWalk);
    }

    private void OnEnable()
    {
        _state.OnStateChanged += StateChanged;
    }

    private void OnDisable()
    {
        _state.OnStateChanged -= StateChanged;
    }

    private void StateChanged(EnemyState.EState state) // 기본Patrol -> 적감시 Detect 1초후 -> Chase 5초경과 -> ChaseFail -> Patrol
    {
        switch (state)
        {
            case EnemyState.EState.None:
                _anim.SetBool(_hashWalk, false);
                break;

            case EnemyState.EState.Patrol:
                _anim.SetBool(_hashWalk, true);
                break;

            case EnemyState.EState.Detect:
                // 딱히 감지 애니메이션은 없는듯 (! 이미지 띄우고 적을 처다보는 정도?)
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
                _anim.SetTrigger(_hashAttack);
                break;

            case EnemyState.EState.Dead:
                _anim.SetTrigger(_hashDead);
                break;
        }
    }
}