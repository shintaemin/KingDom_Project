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
    [SerializeField] private GameObject _killIcon;
    [SerializeField] private TextMeshProUGUI _killText;
    [Header("Rescue")]
    [SerializeField] private GameObject _rescueIcon;
    [SerializeField] private TextMeshProUGUI _rescueText;
    [Header("Goal")]
    [SerializeField] private GameObject _GoalIcon;
    [Header("SubStage")]
    [SerializeField] private GameObject _subStageUI;
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
        if (_killIcon.IsNull("_killIcon") ||
            _killText.IsNull("_killText") ||
            _rescueIcon.IsNull("_rescueIcon") ||
            _rescueText.IsNull("_rescueText") ||
            _GoalIcon.IsNull("_GoalIcon") ||
            _subStageUI.IsNull("_subStageUI")
            )
        {
            return;
        }
        _killIcon.SetActive(false);
        _killText.gameObject.SetActive(false);
        _rescueIcon.SetActive(false);
        _rescueText.gameObject.SetActive(false);
        _GoalIcon.SetActive(false);
        _subStageUI.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void SetMaxSubStage(int value)
    {
        _maxSubStage = value;
        if (_maxSubStage > 1)
            _subStageUI.SetActive(true);

    }
    private void SetCurrentSubStage(int value)
    {
        _currentSubStage = value;
    }
    //==================================================================

    public bool SetMissionType(EMissionType missionType)
    {
        if (_missionType == missionType) return false;
        _missionType = missionType;

        switch (missionType)
        {
            case EMissionType.Kill:
                _killIcon.SetActive(false);
                _killText.gameObject.SetActive(false);
                break;
            case EMissionType.Rescue:
                _rescueIcon.SetActive(false);
                _rescueText.gameObject.SetActive(false);
                break;
            case EMissionType.Goal:
                _GoalIcon.SetActive(false);
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

