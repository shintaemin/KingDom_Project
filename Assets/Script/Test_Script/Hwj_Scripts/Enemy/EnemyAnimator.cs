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
    #endregion

    #region 내부 변수
    private EnemyState _state;
    private EnemyCombat _combat;
    private Animator _anim;
    private NavMeshAgent _nav;
    private int _hashSpeed;
    private int _hashAttack;
    #endregion

    private void Awake()
    {
        _state = GetComponent<EnemyState>();
        _combat = GetComponent<EnemyCombat>();
        _anim = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();

        if (_state == null || _anim == null || _combat == null || _nav == null)
        {
            Debug.LogError("EnemyAnimator _state _anim _combat _nav 참조 실패");
            return;
        }

        _hashSpeed = Animator.StringToHash(_paramSpeed);
        _hashAttack = Animator.StringToHash(_paramAttack);
    }

    private void OnEnable()
    {
        if (_combat != null)
        {
            _combat.OnAttacked += Attacked;
        }
    }

    private void OnDisable()
    {
        if (_combat != null)
        {
            _combat.OnAttacked -= Attacked;
        }
    }

    void Update()
    {
        float CurrentSpeed = _nav.velocity.magnitude;

        _anim.SetFloat(_hashSpeed, CurrentSpeed);
    }

    private void Attacked()
    {
        _anim.SetTrigger(_hashAttack);
    }
}