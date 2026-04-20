using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region UI Glow 잔상 제거
/*
 ▶할일

  - 박라희
*/
#endregion


public class Up_AnimationReset : MonoBehaviour
{
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (_anim == null) return;

        _anim.Rebind();
        _anim.Play("Idle", 0, 0f);
        _anim.Update(0f);
    }

    /// 버튼에서 호출
    public void PlayEffect()
    {
        if (_anim == null) return;

        _anim.Play("UpgradeEffect", 0, 0f);
    }
}
