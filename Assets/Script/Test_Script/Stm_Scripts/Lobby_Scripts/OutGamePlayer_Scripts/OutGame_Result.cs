using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 아웃게임플레이어 결과 애니메이션 지정
/*
 ▶ 할일
  - 결과에 따른 애니메이션 지정
*/
#endregion


public class OutGame_Result : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private IngameManager _ig;
    [SerializeField] private OutGame_Anim _anim;
    #endregion

    #region

    #endregion

    private void OnEnable()
    {
        if (_ig != null)
        {
            _ig.MissionEnd += SetMissionAnswerAnim;
        }
    }

    private void OnDisable()
    {
        if (_ig != null)
        {
            _ig.MissionEnd -= SetMissionAnswerAnim;
        }
    }

    private void SetMissionAnswerAnim(EMissionAnswer answer)
    {
        OutGame_Anim.EOutGameAnimType type = OutGame_Anim.EOutGameAnimType.None;

        type = answer == EMissionAnswer.Success ? OutGame_Anim.EOutGameAnimType.Walk : OutGame_Anim.EOutGameAnimType.Dead;

        _anim.SetTriggerAnim(type);
    }
}
