using TMPro;
using UnityEngine;
using UnityEngine.UI;


#region CStageGoal
/*

*/
#endregion

public partial class CStageGoal : MonoBehaviour
{
    #region 인스펙터

    [Header("확인용. 직접 수정 비추")]
    [SerializeField] private EMissionType? _missionType = null;
    #endregion

    #region 내부 변수
    private IFSM _currentGoalState;
    #endregion

    public EMissionType? MissionType
    {
        get { return _missionType; }
        set { _missionType = value; }
    }

    void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        _currentGoalState.Update();
    }

    public bool SetMissionType(EMissionType missionType)
    {
        if(_missionType != null)
        {
            _currentGoalState.Exit();
        }

        _missionType = missionType;
        _currentGoalState = missionType switch
        {
            EMissionType.Kill => (IFSM)new CStageGoalKill(),
            EMissionType.Rescue => (IFSM)new CStageGoalRescue(),
            //EMissionType.Goal => (IGoalState)new CStageGoalGoal(),
            _ => null
        };

        _currentGoalState.Enter();

        return _currentGoalState != null;
    }
}
