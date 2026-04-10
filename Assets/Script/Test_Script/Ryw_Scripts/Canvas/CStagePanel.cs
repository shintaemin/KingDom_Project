using TMPro;
using UnityEngine;


#region CStagePanel
/*


※ 이 판넬은 스테이지의 서브스테이지 수와 관계 없이 첫번째 방에만 나온다.
따라서 처음 한번만 값을 설정해 주면 된다.
*/
#endregion

public class CStagePanel : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _explanationText;

    [Header("확인용. 직접 수정 비추")]
    [SerializeField] private EMissionType? _missionType = null;
    #endregion

    #region 내부 변수

    #endregion

    public EMissionType? MissionType
    {
        get { return _missionType; }
        set { _missionType = value; }
    }

    void Awake()
    {
        if (_levelText.IsNull("_levelText") ||
            _explanationText.IsNull("_explanationText"))
        {
            return;
        }

    }

    public void SetTextes(int level, EMissionType type)
    {
        SetTextes(level);
        _levelText.text = $"Level {level}";

        switch (type)
        {
            case EMissionType.Kill:
                _explanationText.text = "Eliminate all the officers.";
                break;
            case EMissionType.Rescue:
                _explanationText.text = "Save everyone.";
                break; 
            case EMissionType.Goal:
                _explanationText.text = "Reach the target point.";
                break;
            default:
                _explanationText.text = "Error : Something is wrong.";
                break;
        }
    }

    // 필요할까?
    public void SetTextes(int level)
    {
        if (_missionType == null)
        {
            Debug.LogWarning("_missionType == null");
            return;
        }

        SetTextes(level, _missionType.Value);
    }
}
