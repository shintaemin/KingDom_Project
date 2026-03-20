using UnityEngine;


#region CMissionDataSO
/*
▶ 작성자 류연우
관련 정보로 많은 제보 바랍니다.
https://www.notion.so/328d50353449801784e7c58b2ac68d38?v=328d50353449807699de000cc25c7fb1&p=328d5035344980e78625c4d567cd7ee9&pm=s
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
