using System;
using UnityEngine;
using UnityEngine.UI;


#region FullscreenImpact
/*
▶ 작성자 류연우

*/
#endregion

public class CFullscreenImpact : MonoBehaviour
{
    #region 인스펙터
    [Header("UI 오브젝트")]
    [SerializeField] private Image _impactImage;
    [Header("애니메이션 관련")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramAnimationSpeed = "fAnimationSpeed";
    [SerializeField] private string _paramImpactTrigger = "ImpactTrigger";
    [Header("설정값")]
    //[ContextMenuItem("적용", "SetAnimationSpeed")]
    [SerializeField] private float _animationSpeed = 2f;
    #endregion

    #region 내부 변수
    private int _hashAnimationSpeed;
    private int _hashImpactTrigger;

    #endregion

    void Awake()
    {
        if(_impactImage.IsNull("_impactImage"))
        {
            return;
        }
        _hashAnimationSpeed = Animator.StringToHash(_paramAnimationSpeed);
        _hashImpactTrigger = Animator.StringToHash(_paramImpactTrigger);
        _animator.SetFloat(_hashAnimationSpeed, _animationSpeed);
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
    public void CallImpact(Color color)
    {
        _impactImage.color = color;
        _animator.SetTrigger(_hashImpactTrigger);
    }
}
