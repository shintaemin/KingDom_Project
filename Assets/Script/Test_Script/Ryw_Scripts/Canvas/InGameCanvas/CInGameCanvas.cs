using System.Collections.Generic;
using UnityEngine;


#region CInGameCanvas
/*
▶ 작성자 류연우

인게임 캔버스.
판넬의 활성화의 경우 Phase 클래스들의 엔터에서 활성화.

특정 페이즈에서만 필요한 무언가가 있다면 
이벤트의 구독과 해체의 경우도 enter exit에서 하는게 좋을 것 같다.




첫방에만 스테이지 판넬이 나옴.

적 사망등 이벤트 발생 시 스테이지 골 판넬의 메시지 변경
// 적 사망시 이벤트 : EnemyState.OnDead

모든 적 사망 시
    다음 방이 있다면, GO 판넬 활성화.
    없다면 종료 판넬 출력
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
    [SerializeField] private GameObject _goImpact;
    [SerializeField] private Victory_Panel_Controller _victoryPanel;
    [SerializeField] private Victory_Panel_Controller _failurePanel;

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
    private Transform _playerTransform;

    private EGamePhase _currentGamePhase = EGamePhase.None;
    private IInGameCanvasPhaseFSM _fsm;

    private readonly Dictionary<EGamePhase, IInGameCanvasPhaseFSM> _phaseDic = new Dictionary<EGamePhase, IInGameCanvasPhaseFSM>();

    private IngameManager _ingameManager;
    private int _currentSubStage;
    #endregion

    #region 프로퍼티
    // 이렇게 다 나누지 말고 판넬 딕셔너리를 만드는게 좋아보인다.
    public CStagePanel StagePanel => _stagePanel;
    public CFullscreenImpact FullscreenImpact => _fullscreenImpact;
    public CStageGoal StageGoal => _stageGoal;
    public GameObject GoImpact => _goImpact;
    public Victory_Panel_Controller VictoryPanel => _victoryPanel;
    public Victory_Panel_Controller FailurePanel => _failurePanel;

    public int CurrentSubStage => _currentSubStage;

    // 이건 필요한가?
    public EMissionType? MissionType
    {
        get { return _missionType; }
        set
        {
            SetMissionType(value);
        }
    }
    #endregion


    void Awake()
    {
        if (_stagePanel.IsNull("_stagePanel") ||
            _fullscreenImpact.IsNull("_fullscreenImpact") ||
            _stageGoal.IsNull("_stageGoal") ||
            _instancePanel.IsNull("_instancePanel") ||
            _goImpact.IsNull("_goImpact") ||
            _victoryPanel.IsNull("_victoryPanel") ||
            _failurePanel.IsNull("_failurePanel"))
        {
            return;
        }
        // 외부로부터 받아와야하는데...
        // 외부에서 받아오도록 만들고 다 함수로 만든다.
        MakePhaseDic();

        GameObject tgo = GameObject.Find("InGameManager");

        if (tgo.TryGetComponent(out IngameManager IGM))
        {
            _ingameManager = IGM;
        }
    }
    private void OnEnable()
    {
        _ingameManager.MissionEnd += _ingameManager_MissionEnd;
    }

    private void OnDisable()
    {
        _ingameManager.MissionEnd -= _ingameManager_MissionEnd;
    }
    private void _ingameManager_MissionEnd(EMissionAnswer obj)
    {
        // 모든 판넬 비활성화
        StagePanel.gameObject.SetActive(false);
        StageGoal.gameObject.SetActive(false);
        FullscreenImpact.gameObject.SetActive(false);
        GoImpact.gameObject.SetActive(false);
        // 조건분기
        switch (obj)
        {
            case EMissionAnswer.Success:
                VictoryPanel.gameObject.SetActive(true);
                break;
            case EMissionAnswer.Fail:
                FailurePanel.gameObject.SetActive(true);
                break;
            case EMissionAnswer.None:
                break;
            default:
                break;
        }
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

        _fsm?.Update(this);
    }

    // 이것들은 어디어디 필요하지?

    private void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _instancePanel.PlayerTransform = playerTransform;

    }
    private void SetLevel(int level)
    {
        _stagePanel.SetTextes(level);

    }
    private void SetMaxSubStage(int maxSubStage)
    {
        _stageGoal.MaxSubStage = maxSubStage;
    }
    private void SetCurrentSubStage(int currentSubStage)
    {
        _currentSubStage = currentSubStage;
        _stageGoal.CurrentSubStage = currentSubStage;
    }
    private void SetMissionType(EMissionType? type)
    {
        _missionType = type;
        _stagePanel.MissionType = _missionType;
        _stageGoal.MissionType = _missionType.Value;
    }
    private bool ChangeGamePhase(EGamePhase phase)
    {
        if (phase == EGamePhase.None)
        {
            Debug.LogWarning("이 페이즈는 아직 진입을 염두하지 않았다.");
            return false;
        }
        if (_currentGamePhase == phase)
            return false;


        // _fsm?.Exit(this); 로 대체하고 로그는 다른 방식으로 찍기...
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
    //========================================================================================================================
    // 외부 호출 함수

    // 여기 들어가는 값들은 아마 해당 레벨이 끝날때까지 변하지 않는 값이다.
    public void Init(Transform playerTransform, int level, int subStage)
    {
        SetPlayerTransform(playerTransform);

        SetLevel(level);
        // 서브스테이지 수
        SetMaxSubStage(subStage);

    }

    // 서브스테이지 마다 불러줘야 한다.
    public void Standby(int currentSubStage, EMissionType type)
    {
        // 미션 타입
        MissionType = type;
        SetCurrentSubStage(currentSubStage);
        ChangeGamePhase(EGamePhase.StandbyPhase);   // 이건 어디로?
    }

    // 화면에 임팩트 효과를 줍니다. 색 지정.
    public void CallFullscreenImpact(Color color)
    {
        _fullscreenImpact.CallImpact(color);
    }

    // 화면에 아이콘을 생성합니다. type에 따라 다르게 동작합니다.
    public void SpwanIcon(CInstancePanel.EIconType type, Vector3 position)
    {
        _instancePanel.SpawnIcon(type, position);
    }

    // 화면에 숫자를 생성합니다. 값, 색, 위치를 지정
    public void SpawnNumber(string number, Color color, Vector3 position)
    {
        _instancePanel.SpawnNumber(number, color, position);
    }

    // 0/3 이런 값을 넣어주면 됩니다.
    public void SetGoalText(string goalText)
    {
        _stageGoal.SetText(goalText);
    }

    // Go!
    public void SetActiveGoImpact(bool flag)
    {
        _goImpact.SetActive(flag);
        if (flag)
            CallFullscreenImpact(Color.white);
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
    public static Transform GetSpawnRootTransform()
    {
        return GameObject.Find("SpawnRoot").transform;
    }
}
