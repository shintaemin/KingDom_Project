using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 아웃 게임 애니메이션
/*
 ▶ 할일
  - 아웃 게임 플레이어 애니메이션 지정 로직
  - 컨트롤러를 캐싱하고 지정이 가능하도록 작업
*/
#endregion


public class OutGame_Anim : MonoBehaviour
{
    public enum EOutGameAnimType
    {
        None,
        Idle,
        Walk,
        Hit,
        Dead,
        Upgrade,
    }

    #region 인스펙터
    [SerializeField] private Animator _anim;
    [SerializeField] private string _tWalkParam = "tWalk";
    [SerializeField] private string _tHit01_Param = "tHit01";
    [SerializeField] private string _tHit02_Param = "tHit02";
    [SerializeField] private string _tDeadParam = "tDead";
    [SerializeField] private string _tUpgradeParam = "tUpgrade";

    [Header("테스트용 - 변경시 애니메이션 재생 (Hit, Dead(1Way), Walk(1Way)")]
    [SerializeField] private EOutGameAnimType testType = EOutGameAnimType.None;
    #endregion

    #region 내부 변수
    private int _walkHash;
    private int _hit01_Hash;
    private int _hit02_Hash;
    private int _deadHash;
    private int _upgradeHash;
    #endregion

    private void OnValidate()
    {
        SetTriggerAnim(testType);
    }

    private void Awake()
    {
        if (_anim == null)
        {
            if (!TryGetComponent<Animator>(out _anim))
            {
                Debug.LogWarning($"[OutGame_Anim] : 애니메이터 캐싱 실패");
                return;
            }
        }

        _walkHash = Animator.StringToHash(_tWalkParam);
        _hit01_Hash = Animator.StringToHash(_tHit01_Param);
        _hit02_Hash = Animator.StringToHash(_tHit02_Param);
        _deadHash = Animator.StringToHash(_tDeadParam);
        _upgradeHash = Animator.StringToHash(_tUpgradeParam);
    }

    #region 외부 호출 함수
    public void SetTriggerAnim(EOutGameAnimType type)
    {
        if (_anim == null)
        {
            return;
        }

        int current = 0;

        switch(type)
        {
            case EOutGameAnimType.Walk:
                current = _walkHash;
                break;
            case EOutGameAnimType.Dead:
                current = _deadHash;
                break;
            case EOutGameAnimType.Upgrade:
                current = _upgradeHash;
                break;
            case EOutGameAnimType.Hit:
                int rand = Random.Range(0, 2);
                current = rand == 0 ? _hit01_Hash : _hit02_Hash;
                break;
        }

        if (current != 0)
        {
            _anim.SetTrigger(current);
        }
    }
    #endregion
}
