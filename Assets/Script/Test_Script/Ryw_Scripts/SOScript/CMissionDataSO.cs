using UnityEngine;


#region CMissionDataSO
/*
▶ 작성자 류연우
// 피해 없이 레벨 클리어의 경우 보상의 값을 모른다. 일단 999 입력함.
*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Mission Data (SO)", fileName = "MissionDataSO_")]
public class CMissionDataSO : ScriptableObject
{
    public enum EMissionType
    {
        LevelClear,
        KillMonster,
        Jewel,
        NoHit,
        CompleteMission,
        Advertising
    }
    #region 인스펙터
    [SerializeField] private int _ID = 000;
    [SerializeField] private EMissionType _type = EMissionType.LevelClear;
    [SerializeField] private string _name = "레벨 클리어";
    [SerializeField] private int _condition = 3;
    [SerializeField] private int _reward = 250;
    #endregion

    #region 프로퍼티
    public int ID =>_ID;
    public EMissionType Type => _type;
    public string Name => _name;
    public int Condition => _condition;
    public int Reward => _reward;
    #endregion

}
