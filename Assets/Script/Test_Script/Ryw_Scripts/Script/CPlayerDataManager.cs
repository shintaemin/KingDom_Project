using UnityEngine;


#region CPlayerDataManager
/*
▶작성자 류연우

일단 확인을 위해 인스펙터로 뺀다.
나중에 변경하더라도


이건 싱글톤이여야 할까?
*/
#endregion

[System.Serializable]
public class PlayerSaveData
{
    public int Gem;
    public int Energy;

    public int CurrentWeaponID;
    public int CurrentClothesID;

    public int[] CurrentUpgradeLevel = new int[3];
    public int[] CurrentTalentLevel = new int[9];
}

public class CPlayerDataManager : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [SerializeField] private int _gem;
    [SerializeField] private int _energy;

    [SerializeField] private int _currentWeaponID;
    [SerializeField] private int _currentClothesID;

    [SerializeField] private int[] _currentUpgradeLevel = new int[3];
    [SerializeField] private int[] _currentTalentLevel = new int[9];
    #endregion

    #region 내부 변수
    private PlayerSaveData _data;
    #endregion
    public object SaveData { get => _data; set => _data = (PlayerSaveData)value; }

    void Awake()
    {

    }

    void Start()
    {
        CJsonManager.Instance.Add("playerData", this, typeof(PlayerSaveData));
    }

    void Update()
    {
        
    }

    public void MakeSaveData()
    {
        if (_data == null)
            _data = new PlayerSaveData();
        
        _data.Gem = _gem;
        _data.Energy = _energy;

        _data.CurrentWeaponID = _currentWeaponID;
        _data.CurrentClothesID = _currentClothesID;

        _data.CurrentUpgradeLevel = _currentUpgradeLevel;
        _data.CurrentTalentLevel = _currentTalentLevel;   
    }

    public void LoadSaveData()
    {
        if (_data.IsNull("_data"))
            return;

        _gem = _data.Gem;
        _energy = _data.Energy;

        _currentWeaponID = _data.CurrentWeaponID;
        _currentClothesID = _data.CurrentClothesID;

        _currentUpgradeLevel = _data.CurrentUpgradeLevel;
        _currentTalentLevel = _data.CurrentTalentLevel;
    }
}
