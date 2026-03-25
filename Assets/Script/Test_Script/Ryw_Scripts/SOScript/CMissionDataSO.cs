using System;
using UnityEditor;
using UnityEngine;
using static CEquipmentDataSO;


#region CMissionDataSO
/*
▶ 작성자 류연우
관련 정보로 많은 제보 바랍니다.
https://www.notion.so/328d50353449801784e7c58b2ac68d38?v=328d50353449807699de000cc25c7fb1&p=328d5035344980e78625c4d567cd7ee9&pm=s
*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Mission Data (SO)", fileName = "MissionDataSO_")]
public class CMissionDataSO : ScriptableObject, ICVSData
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

    public void ParseData(string data)
    {
        string[] dataArr = data.Split(",");

        _ID = int.Parse(dataArr[0]);
        _type = (EMissionType)Enum.Parse(typeof(EMissionType), dataArr[1]);
        _name = dataArr[2];
        _condition = int.Parse(dataArr[3]);
        _reward = int.Parse(dataArr[4]);

        string path = $"Assets/Script/Test_Script/Ryw_Scripts/MissionData/MissionDataSO_{_ID}.asset";

        AssetDatabase.CreateAsset(this, path);
        AssetDatabase.SaveAssets();
    }
    #endregion

}
