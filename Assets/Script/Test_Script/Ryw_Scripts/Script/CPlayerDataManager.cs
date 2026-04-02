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

    // 이 부분은 고민이 필요하다.
    // 딕셔너리로 저장하면 편하겠지만, 직렬화를 위한 추가 처리가 필요하다.
    // UI를 기준으로 정한다면 그냥 배열로 하면 되지만, UI와의 지속적인 연동이 필요한데... 이러면 책임 역전 아닌가?
    //[SerializeField] private bool[] _weaponUnLock;
    //[SerializeField] private bool[] _ClothesUnLock;
    #endregion

    #region 내부 변수
    // 저장 직전 자신의 데이터를 덮어씌우는 부분이 있긴 하지만
    // 실제 저장 / 불러오기는 이 객체를 기준으로 이루어 진다.
    private PlayerSaveData _data;
    #endregion

    #region 프로퍼티
    public int Gem => _gem;
    public int Energy => _energy;
    public int CurrentWeaponID => _currentWeaponID;
    public int CurrentClothesID => _currentClothesID;

    public int[] CurrentUpgradeLevel => _currentUpgradeLevel;
    public int[] CurrentTalentLevel => _currentTalentLevel;

    public int CurrentUpgradeSum
    {
        get
        {
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += _currentUpgradeLevel[i];
            }

            return sum;
        }
    }
    public int CurrentTalentSum
    {
        get
        {
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += _currentTalentLevel[i];
            }

            return sum;
        }
    }
    public object SaveData { get => _data; set => _data = (PlayerSaveData)value; }
    #endregion

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

    public bool TryUseEnergy(int energy)
    {
        if (_energy >= energy)
        {
            _energy -= energy;
            return true;
        }
        return false;
    }

    public bool TryUseGem(int gem)
    {
        if (_gem >= gem)
        {
            _gem -= gem;
            return true;
        }
        return false;
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
