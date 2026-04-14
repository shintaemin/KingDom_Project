using System;
using TMPro;
using UnityEngine;


#region CStageGoal
/*


// 미션 타입과
// 최대 스테이지를 입력받아 UI를 만든다.

// 현제 스테이지에 맞게 변경한다.
*/
#endregion

public partial class CStageGoal : MonoBehaviour
{
    #region 인스펙터
    [Header("확인용. 직접 수정 비추")]
    [SerializeField] private EMissionType? _missionType = null;

    [Header("Kill")]
    [SerializeField] private GameObject _killUI;
    [SerializeField] private TextMeshProUGUI _killText;
    [Header("Rescue")]
    [SerializeField] private GameObject _rescueUI;
    [SerializeField] private TextMeshProUGUI _rescueText;
    [Header("Goal")]
    [SerializeField] private GameObject _GoalUI;
    [Header("SubStage")]
    [SerializeField] private CSubStage _subStageUI;

    [Header("디버그")]
    [SerializeField] bool _useDebugKey = false;
    #endregion

    #region 내부 변수
    private int _maxSubStage;
    private int _currentSubStage;
    #endregion

    public EMissionType MissionType
    {
        get { return _missionType.Value; }
        set { SetMissionType(value); }
    }

    public int MaxSubStage
    {
        get { return _maxSubStage; }
        set
        {
            SetMaxSubStage(value);
        }
    }

    public int CurrentSubStage
    {
        get { return _currentSubStage; }
        set
        {
            SetCurrentSubStage(value);
        }
    }


    void Awake()
    {
        if (_killUI.IsNull("_killUI") ||
            _killText.IsNull("_killText") ||
            _rescueUI.IsNull("_rescueUI") ||
            _rescueText.IsNull("_rescueText") ||
            _GoalUI.IsNull("_GoalUI") ||
            _subStageUI.IsNull("_subStageUI")
            )
        {
            return;
        }
        _killUI.SetActive(false);
        //_killText.gameObject.SetActive(false);
        _rescueUI.SetActive(false);
        //_rescueText.gameObject.SetActive(false);
        _GoalUI.SetActive(false);
        _subStageUI.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {
        if (_useDebugKey)
        {
            if (_maxSubStage == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    int key = (int)KeyCode.Alpha1 + i;
                    if (Input.GetKeyDown((KeyCode)key))
                    {
                        SetMaxSubStage(i + 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    int key = (int)KeyCode.Alpha1 + i;
                    if (Input.GetKeyDown((KeyCode)key))
                    {
                        SetCurrentSubStage(i + 1);
                    }
                }
            }
        }
    }

    private void SetMaxSubStage(int value)
    {
        _maxSubStage = value;
        if (_maxSubStage > 1)
        {
            _subStageUI.gameObject.SetActive(true);
            // 여기서 UI를 만든다.
            _subStageUI.MaxSubStage = _maxSubStage;
        }
    }
    private void SetCurrentSubStage(int value)
    {
        _currentSubStage = value;
        // 여기서 UI의 상태를 바꾼다.
        _subStageUI.CurrentSubStage = _currentSubStage;
    }
    //==================================================================

    public bool SetMissionType(EMissionType missionType)
    {
        if (_missionType == missionType) return false;
        _missionType = missionType;

        switch (missionType)
        {
            case EMissionType.Kill:
                _killUI.SetActive(false);
                //_killText.gameObject.SetActive(false);
                break;
            case EMissionType.Rescue:
                _rescueUI.SetActive(false);
                //_rescueText.gameObject.SetActive(false);
                break;
            case EMissionType.Goal:
                _GoalUI.SetActive(false);
                break;
        }

        return _missionType != null;
    }

    public void SetText(string text)
    {
        switch (_missionType)
        {
            case EMissionType.Kill:
                _killText.text = text;
                break;
            case EMissionType.Rescue:
                _rescueText.text = text;
                break;
            case EMissionType.Goal:
                Debug.LogWarning("골 미션에는 텍스트가 없다.");
                break;
        }
    }
}

