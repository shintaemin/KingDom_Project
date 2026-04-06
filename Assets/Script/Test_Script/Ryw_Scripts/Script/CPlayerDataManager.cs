using System.Collections.Generic;
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

    public int CurrentStage;

    public int[] CurrentUpgradeLevel = new int[3];
    public int[] CurrentTalentLevel = new int[9];

    // 딕셔너리
    // 2026-04-02 기준 장비의 총 개수는 64개임. 데이터 시트 참고.
    public int[] EquipmentDicID = new int[64];
    public bool[] EquipmentDicValue = new bool[64];
}

public class CPlayerDataManager : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [Header("저장될 정보")]
    [SerializeField] private int _gem;
    [SerializeField] private int _energy;

    [SerializeField] private int _currentWeaponID;
    [SerializeField] private int _currentClothesID;

    [SerializeField] private int _currentStage;

    [SerializeField] private int[] _currentUpgradeLevel = new int[3];
    [SerializeField] private int[] _currentTalentLevel = new int[9];

    [Header("플레이어 캐릭터 관련 기본값")]
    [SerializeField] private int _defaultAttack = 100;
    [SerializeField] private float _defaultCriticalRate = 1.05f;
    [SerializeField] private int _defaultDefence = 10;
    [SerializeField] private int _defaultHp = 100;
    [SerializeField] private int _defaultRecovery = 10;
    [SerializeField] private float _defaultMoveSpeed = 1f;


    //[Header("디버그용. 추후 [SerializeField]를 제거하고 내부변수쪽으로 옮긴다.")]
    //[SerializeField] private PlayerSaveData _data;
    #endregion

    #region 내부 변수
    private readonly Dictionary<int, bool> _equipmentUnLockDic = new Dictionary<int, bool>();

    // 저장 직전 자신의 데이터를 덮어씌우는 부분이 있긴 하지만
    // 실제 저장 / 불러오기는 이 객체를 기준으로 이루어 진다.
    private PlayerSaveData _data;

    private CEquipmentDataSO _currentWeapon;
    private CEquipmentDataSO _currentClothes;


    #endregion

    #region 프로퍼티
    public static CPlayerDataManager Instance;

    public int Gem => _gem;
    public int Energy => _energy;
    public int CurrentWeaponID
    {
        get
        {
            return _currentWeaponID;
        }
        set
        {
            if (value >= 1000)
            {
                Debug.LogWarning($"CurrentWeaponID에 입력한 ID 값이 1000 이상입니다. {value}");
                return;
            }
            _currentWeaponID = value;
            _currentWeapon = (CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][_currentWeaponID] as CEquipmentDataSO);
        }
    }
    public int CurrentClothesID
    {
        get
        {
            return _currentClothesID;
        }
        set
        {
            if (value < 1000)
            {
                Debug.LogWarning($"CurrentClothesID에 입력한 ID 값이 1000 미만입니다. {value}");
                return;
            }
            _currentClothesID = value;
            _currentClothes = (CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][_currentClothesID] as CEquipmentDataSO);
        }
    }

    public int CurrentStage => _currentStage;

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
    // 언락 관련
    public Dictionary<int, bool> EquipmentUnLockDic => _equipmentUnLockDic;

    public int UnLockedWeaponCount
    {
        get
        {
            return 0;
        }
    }

    public int UnLockedClothesCount
    {
        get
        {
            return 0;
        }
    }
    // playerCharacter
    // 캐싱 권장.
    // 간결화 하지 말고 작성 권장.
    public int Attack
    {
        get
        {
            int result = _defaultAttack;
            result += _currentWeapon.AdditionalAtt;
            // 재능 / 등으로 얻는 수치 포함

            // 배율 설정.
            return result;
        }
    }
    // 재능으로 얻는 수치 
    public float CriticalRate
    {
        get
        {
            float result = _defaultCriticalRate;
            // 재능 / 등으로 얻는 수치 포함
            return result;
        }
    }
    // 추가 공격력
    public int CriticalAttack => (int)(Attack * 1.5f);

    // 추가 공격력
    public int BackAttackAttack => (int)(Attack * 1.3f);

    public int Defence => _defaultDefence;

    public int HP => _defaultHp;

    public int Recovery => _defaultRecovery;

    public float MoveSpeed => _defaultMoveSpeed;

    public object SaveData { get => _data; set => _data = (PlayerSaveData)value; }
    #endregion

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance != null && Instance != this");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CJsonManager.Instance.Add("playerData", this, typeof(PlayerSaveData));
    }

    void Update()
    {

    }

    public void ChangeCurrentEquipment(int id)
    {

    }
    private void ChangeCurrentClothes(int id)
    {

    }

    private void ChangeCurrentWeapon(int id)
    {

    }

    public void ChangeUnLockDic(int key, bool value)
    {

    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
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

        _data.CurrentStage = _currentStage;

        _data.CurrentUpgradeLevel = _currentUpgradeLevel;
        _data.CurrentTalentLevel = _currentTalentLevel;

        _equipmentUnLockDic.DicToArray(_data.EquipmentDicID, _data.EquipmentDicValue);
    }

    public void LoadSaveData()
    {
        if (_data.IsNull("_data"))
            return;

        _gem = _data.Gem;
        _energy = _data.Energy;

        _currentWeaponID = _data.CurrentWeaponID;
        _currentClothesID = _data.CurrentClothesID;

        _currentStage = _data.CurrentStage;

        _currentUpgradeLevel = _data.CurrentUpgradeLevel;
        _currentTalentLevel = _data.CurrentTalentLevel;

        _equipmentUnLockDic.ArrayToDic(_data.EquipmentDicID, _data.EquipmentDicValue);
    }
}
