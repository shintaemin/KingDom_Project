using UnityEngine;


#region CStageGoal
/*

*/
#endregion

public partial class CStageGoal : MonoBehaviour
{
    #region 인스펙터
    [Header("확인용. 직접 수정 비추")]
    [SerializeField] private EMissionType? _missionType = null;

    [SerializeField] private int _subStageNum;  // 이게 이번 레벨의 스테이지 개수임.
    [SerializeField] private int _currentSubStageNum;
    #endregion

    #region 내부 변수
    private IGoalState _currentGoalState;
    #endregion

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
            EMissionType.Kill => (IGoalState)new CStageGoalKill(),
            EMissionType.Rescue => (IGoalState)new CStageGoalRescue(),
            //EMissionType.Goal => (IGoalState)new CStageGoalGoal(),
            _ => null
        };

        _currentGoalState.Enter();

        return _currentGoalState != null;
    }
}
