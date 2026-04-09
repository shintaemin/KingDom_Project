using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 도어 오픈 애니메이션
/*
 ▶ 할일
  - 미션이 클리어되면 인게임매니저를 통해 PlayOpenAnim 을 호출하고 애니메이션 재생
*/
#endregion


public class DoorOpenAnim : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Animator _anim;
    [SerializeField] private Door_StageEnd_Col _col;
    [SerializeField] private string _openingParam = "tOpening";
    #endregion

    #region 내부 변수
    private int _openingTrigerHash;
    #endregion

    private void Awake()
    {
        if (_anim == null)
        {
            if (!TryGetComponent<Animator>(out _anim))
            {
                Debug.LogWarning($"[DoorOpenAnim] : 애니메이터 참조 실패");
                return;
            }
        }

        if (_col == null)
        {
            if (!TryGetComponent<Door_StageEnd_Col>(out _col))
            {
                Debug.LogWarning($"[DoorOpenAnim] : 콜라이더 참조 실패");
                return;
            }
        }

        _openingTrigerHash = Animator.StringToHash(_openingParam);
    }

    #region 외부 호출 함수
    public void PlayOpenAnim()
    {
        if (_anim == null)
        {
            return;
        }

        _col.SetTrigger(true);
        _anim.SetTrigger(_openingTrigerHash);
    }
    #endregion
}
