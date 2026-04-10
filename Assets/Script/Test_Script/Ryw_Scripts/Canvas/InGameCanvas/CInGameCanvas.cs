using UnityEngine;


#region CInGameCanvas
/*
▶ 작성자 류연우

인게임 캔버스.
*/
#endregion

public class CInGameCanvas : MonoBehaviour
{
    public enum EGamePhase
    {
        StandbyPhase,
        MainPhase,
        EndPhase
    }
    // ~Step 이라는 이름으로 만든다.
    public enum EStep
    {
        None,
    }
    #region 인스펙터
    [Header("판넬들")]
    [SerializeField] private CStagePanel _stagePanel;
    [SerializeField] private CFullscreenImpact _fullscreenImpact;
    [SerializeField] private CStageGoal _stageGoal;
    //[SerializeField] private Victory_Panel_Controller _victoryPanel;
    //[SerializeField] private Victory_Panel_Controller _FailurePanel;

    [Header("확인용. 직접 수정 비추")]
    [SerializeField] private EMissionType? _missionType = null;

    [Header("디버그")]
    [SerializeField] private bool UseDebugKey = false;
    [SerializeField] private Color ImpactColor = Color.yellow;
    [SerializeField] private KeyCode ImpactKey = KeyCode.I;
    #endregion

    #region 내부 변수
    [SerializeField] private EGamePhase _currentGamePhase = EGamePhase.StandbyPhase;
    #endregion

    public EMissionType? MissionType
    {
        get { return _missionType; }
        set
        {
            _missionType = value;
            _stagePanel.MissionType = _missionType;
            _stageGoal.MissionType = _missionType;
        }
    }

    void Awake()
    {
        if (_stagePanel.IsNull("_stagePanel") ||
            _fullscreenImpact.IsNull("_fullscreenImpact") ||
            _stageGoal.IsNull("_stageGoal"))
        {
            return;
        }

        int level = 0;
        MissionType = _missionType;
        _stagePanel.SetTextes(level/*, _missionType.Value*/); // 윗줄에서 타입을 지정해주면, 따로 해줄 필요는 없다. 불안하면 해줘도 된다. 단, 예외처리 필요.
    }

    void Start()
    {
        _stagePanel.gameObject.SetActive(false);
        _stageGoal.gameObject.SetActive(false);
    }

    void Update()
    {
        // 이걸 업데이트에ㅓㅅ 한다고? 엔터가 아니라?
        switch (_currentGamePhase)
        {
            case EGamePhase.StandbyPhase:
                _stagePanel.gameObject.SetActive(true);
                _stageGoal.gameObject.SetActive(false);
                break;
            case EGamePhase.MainPhase:
                _stagePanel.gameObject.SetActive(false);
                _stageGoal.gameObject.SetActive(true);
                break;
            case EGamePhase.EndPhase:
                //Victory_Panel_Controller 를 조건에 맞게 활성화.
                break;
        }

        if (UseDebugKey)
        {
            if (Input.GetKeyDown(ImpactKey))
            {
                CallFullscreenImpact(ImpactColor);
            }
        }

    }

    public bool ChangeGamePhase(EGamePhase phase)
    {
        if (_currentGamePhase == phase)
            return false;

        _currentGamePhase = phase;
        return true;
    }

    public void CallFullscreenImpact(Color color)
    {
        _fullscreenImpact.CallImpact(color);
    }
}
