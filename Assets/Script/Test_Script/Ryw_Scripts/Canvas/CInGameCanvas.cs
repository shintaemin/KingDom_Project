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

    [Header("디버그")]
    [SerializeField] private bool UseDebugKey = false;
    [SerializeField] private Color ImpactColor = Color.yellow;
    [SerializeField] private KeyCode ImpactKey = KeyCode.I;
    #endregion

    #region 내부 변수
    [SerializeField] private EGamePhase _currentGamePhase = EGamePhase.StandbyPhase;
    #endregion

    void Awake()
    {
        if (_stagePanel.IsNull("_stagePanel") ||
            _fullscreenImpact.IsNull("_fullscreenImpact") ||
            _stageGoal.IsNull("_stageGoal"))
        {
            return;
        }
    }

    void Start()
    {
        _stagePanel.gameObject.SetActive(false);
        _stageGoal.gameObject.SetActive(false);
    }

    void Update()
    {
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
