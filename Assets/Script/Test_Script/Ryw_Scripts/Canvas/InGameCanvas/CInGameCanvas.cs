using System.Collections.Generic;
using UnityEngine;


#region CInGameCanvas
/*
▶ 작성자 류연우

인게임 캔버스.
판넬의 활성화의 경우 Phase 클래스들의 엔터에서 활성화.

구독과 해체의 경우도 엔터에 exit에서 하는게 좋을 것 같다.
*/
#endregion

public class CInGameCanvas : MonoBehaviour
{
    public enum EGamePhase
    {
        None,
        StandbyPhase,
        MainPhase,
        EndPhase
    }

    private readonly CStandbyPhase STANDBYPHASE = new CStandbyPhase();
    private readonly CMainPhase MAINPHASE = new CMainPhase();
    private readonly CEndPhase ENDPHASE = new CEndPhase();

    // ~Step 이라는 이름으로 만든다.
    public enum EStep
    {
        None,
    }
    #region 인스펙터
    // 판넬들을 담는 딕셔너리도 있는게 좋아 보인다.
    [Header("판넬들")]
    [SerializeField] private CStagePanel _stagePanel;
    [SerializeField] private CFullscreenImpact _fullscreenImpact;
    [SerializeField] private CStageGoal _stageGoal;
    [SerializeField] private CInstancePanel _instancePanel;
    //[SerializeField] private Victory_Panel_Controller _victoryPanel;
    //[SerializeField] private Victory_Panel_Controller _FailurePanel;

    [Header("확인용. 직접 수정 비추")]
    [SerializeField] private EMissionType? _missionType = null;

    [Header("디버그")]
    [SerializeField] private bool UseDebugKey = false;
    [SerializeField] private Color ImpactColor = Color.yellow;
    [SerializeField] private KeyCode ImpactKey = KeyCode.I;

    [SerializeField] private KeyCode PhaseChageLeftKey1 = KeyCode.Comma;
    [SerializeField] private KeyCode PhaseChageRightKey2 = KeyCode.Period;
    #endregion

    #region 내부 변수
    private EGamePhase _currentGamePhase = EGamePhase.None;
    private IInGameCanvasPhaseFSM _fsm;

    private readonly Dictionary<EGamePhase, IInGameCanvasPhaseFSM> _phaseDic = new Dictionary<EGamePhase, IInGameCanvasPhaseFSM>();
    #endregion

    #region 프로퍼티
    // 이렇게 다 나누지 말고 판넬 딕셔너리를 만드는게 좋아보인다.
    public CStagePanel StagePanel => _stagePanel;
    public CFullscreenImpact FullscreenImpact => _fullscreenImpact;
    public CStageGoal StageGoal => _stageGoal;
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
            _stageGoal.IsNull("_stageGoal") ||
            _instancePanel.IsNull("_instancePanel"))
        {
            return;
        }

        // 외부로부터 받아와야하는데...
        // 외부에서 받아오도록 만들고 다 함수로 만든다.
        int level = 0;
        // 미션 타입
        MissionType = _missionType;
        // 서브스테이지 수
        _stagePanel.SetTextes(level/*, _missionType.Value*/); // 윗줄에서 타입을 지정해주면, 따로 해줄 필요는 없다. 불안하면 해줘도 된다. 단, 예외처리 필요.

        // 미션 타입과 카운트


        MakePhaseDic();

        ChangeGamePhase(EGamePhase.StandbyPhase);
    }

    private void MakePhaseDic()
    {
        _phaseDic.Clear();

        _phaseDic[EGamePhase.StandbyPhase] = STANDBYPHASE;
        _phaseDic[EGamePhase.MainPhase] = MAINPHASE;
        _phaseDic[EGamePhase.EndPhase] = ENDPHASE;
    }

    void Start()
    {
    }

    void Update()
    {
        if (UseDebugKey)
        {
            if (Input.GetKeyDown(ImpactKey))
            {
                CallFullscreenImpact(ImpactColor);
            }
            if (Input.GetKeyDown(PhaseChageLeftKey1))
            {
                int cur = (int)_currentGamePhase;
                if (cur > 1)
                {
                    cur--;
                    ChangeGamePhase((EGamePhase)cur);
                }
            }
            if (Input.GetKeyDown(PhaseChageRightKey2))
            {
                int cur = (int)_currentGamePhase;
                if (cur < (int)EGamePhase.EndPhase)
                {
                    cur++;
                    ChangeGamePhase((EGamePhase)cur);
                }
            }
        }

        if (_fsm.IsNull("_fsm"))
        {
            return;
        }
        _fsm.Update(this);
    }

    //========================================================================================================================
    // 외부 호출 함수
    public bool ChangeGamePhase(EGamePhase phase)
    {
        if (phase == EGamePhase.None)
        {
            Debug.LogWarning("이 페이즈는 아직 진입을 염두하지 않았다.");
            return false;
        }
        if (_currentGamePhase == phase)
            return false;

        if (_fsm != null)
        {
            Debug.Log($"{_currentGamePhase.ToString()} Exit");
            _fsm.Exit(this);
        }

        _currentGamePhase = phase;
        _fsm = _phaseDic[_currentGamePhase];

        Debug.Log($"{_currentGamePhase.ToString()} Enter");
        _fsm.Enter(this);

        return true;
    }

    public void CallFullscreenImpact(Color color)
    {
        _fullscreenImpact.CallImpact(color);
    }

    public void SpwanIcon(CInstancePanel.EIconType type, Vector3 position)
    {
        _instancePanel.SpawnIcon(type, position);
    }

    public void SpawnNumber(string number, Color color, Vector3 position)
    {
        _instancePanel.SpawnNumber(number, color, position);
    }

    //========================================================================================================================
    // 유틸리티 함수
    public static bool WorldToUI(Vector3 worldPosition, out Vector3 UIPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        if (screenPos.z < 0)
        {
            // 카메라의 뒤에 있다.
            UIPos = Vector3.zero;
            return false;
        }
        UIPos = screenPos;
        return true;
    }
}
