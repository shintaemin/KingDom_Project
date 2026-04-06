using UnityEngine;

#region CTalentDataSO
/*
▶ 작성자 류연우
*/
#endregion


[CreateAssetMenu(menuName = "Create SO/Data/Talent Data (SO)", fileName = "TalentDataSO_")]
public class CTalentDataSO : ScriptableObject, ICSVData
{
    static readonly string NAME = "TalentData";

    // 방어력을 예시로
    #region 인스펙터
    [SerializeField] private int _ID = 0;
    [SerializeField] private string _name = "방어력";
    [SerializeField] private string _information = "방어력 {}상승";
    [SerializeField] private int _basic = 30;
    [SerializeField] private int _volume = 10;
    [SerializeField] private Texture2D _icon;
    #endregion

    #region 프로퍼티
    public int ID => _ID;
    public string Name => _name;
    public string Information => _information;
    public int Basic => _basic;
    public int Volume => _volume;
    public Texture2D Icon => _icon;
    #endregion

    public string ParseData(string data)
    {
        string[] dataArr = data.Split(",");

        _ID = int.Parse(dataArr[0]);
        _name = dataArr[1];
        _information = dataArr[2];
        _basic = int.Parse(dataArr[3]);
        _volume = int.Parse(dataArr[4]);
        _icon = _icon.ParseData(dataArr[5]);

        return CGSSLoader.SOSavePath(NAME) + $"/{NAME}SO_{_ID}.asset";
    }


}
