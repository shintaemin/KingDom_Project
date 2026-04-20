using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region UI Glow 잔상 제거
/*
 ▶할일
  - 탭 이동 시 애니메이션 및 UI 잔상 초기화
  - Animator 상태 초기화
  - UI 이탈 시 초기화 중간 프레임 제거
  - Glow Image 알파값을 0으로 초기화

  - 박라희
*/
#endregion


public class Up_AnimationReset : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Graphic _glowImage;
    #endregion

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // UI 활성화 시 상태 초기화
        ResetState();
    }

    void OnDisable()
    {
        // UI 비활성화 시 상태 초기화
        ResetState();
    }

    void ResetState()
    {
        if (_anim != null)
        {
            _anim.Rebind();
            _anim.Play("Idle", 0, 0f);
            _anim.Update(0f);
        }

        // Glow 알파 강제 초기화
        if (_glowImage != null)
        {
            Color c = _glowImage.color;
            c.a = 0f;
            _glowImage.color = c;
        }
    }

    // 버튼 클릭 시 이펙트 실행
    public void PlayEffect()
    {
        if (_anim == null) return;

        _anim.Play("UpgradeEffect", 0, 0f);
    }
}
