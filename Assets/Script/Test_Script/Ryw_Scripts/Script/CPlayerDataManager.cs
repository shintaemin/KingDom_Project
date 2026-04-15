using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // 라희추가


#region CPlayerDataManager
/*
▶작성자 류연우

일단 확인을 위해 인스펙터로 뺀다.
나중에 변경하더라도


이건 싱글톤이여야 할까?

최대 스테이지인 20을 저장하는 부분은 어디에 있는가?
20 이후로는 어떻게 할 것인가?
*/
#endregion

[System.Serializable]
public class PlayerSaveData
{
    public int MaxGem;
    public int CurrentGem;
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

    public PlayerSaveData()
    {
        CurrentClothesID = 1000;
        // 기본 무기
        EquipmentDicID[0] = 0;
        EquipmentDicValue[0] = true;

        // 기본 스킨
        // '배열의 index'가 중요한게 아니라 '해당 index에 저장된 값'이 중요하다.
        EquipmentDicID[1] = 1000;
        EquipmentDicValue[1] = true;
    }
}

public class CPlayerDataManager : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [Header("저장될 정보")]
    [SerializeField] private int _maxGem;
    [SerializeField] private int _currentGem;
    [SerializeField] private int _energy;

    [SerializeField] private int _currentWeaponID;
    [SerializeField] private int _currentClothesID;

    [SerializeField] private int _currentStage;

    // 리스트로 변경해 프로퍼티에서 readonlyList를 사용하는걸 고려
    [SerializeField] private int[] _currentUpgradeLevel = new int[3];
    [SerializeField] private int[] _currentTalentLevel = new int[9];


    [Header("플레이어 캐릭터 관련 기본값")]
    [SerializeField] private int _defaultAttack = 100;
    [SerializeField] private float _defaultCriticalRate = 0.05f;
    [SerializeField] private int _defaultDefence = 10;
    [SerializeField] private int _defaultHp = 100;
    [SerializeField] private int _defaultRecovery = 10;
    [SerializeField] private float _defaultMoveSpeed = 1f;
    [SerializeField] private float _defaultCriticalAttackRate = 0.5f;
    [SerializeField] private float _defaultBackAttackRate = 0.3f;


    //[Header("디버그용. 추후 [SerializeField]를 제거하고 내부변수쪽으로 옮긴다.")]
    //[SerializeField] private PlayerSaveData _data;
    #endregion

    #region 내부 변수
    private readonly Dictionary<int, bool> _equipmentUnLockDic = new Dictionary<int, bool>();

    // 저장 고려
    private int _unLockedWeaponCount;
    private int _unLockedClothesCount;

    // 저장 직전 자신의 데이터를 덮어씌우는 부분이 있긴 하지만
    // 실제 저장 / 불러오기는 이 객체를 기준으로 이루어 진다.
    private PlayerSaveData _data;

    private CEquipmentDataSO _currentWeapon;
    private CEquipmentDataSO _currentClothes;

    public event Action OnStatChanged; // 라희 추가
    #endregion

    #region 프로퍼티
    public static CPlayerDataManager Instance;

    public int MaxGem
    {
        get { return _maxGem; }
        set { _maxGem = value; }
    }

    /// <summary>
    /// 음수 set의 경우 그냥 TryUseGem를 호출해 사용하는걸 추천.
    /// </summary>
    public int CurrentGem
    {
        get
        {
            return _currentGem;
        }
        set
        {
            if (value > 0)
            {
                _currentGem += value;
            }
            else if (value < 0)
            {
                TryUseGem(value);
            }
        }
    }
    /// <summary>
    /// 음수 set의 경우 그냥 TryUseEnergy를 호출해 사용하는걸 추천.
    /// </summary>

    // 최대 에너지와 현재 에너지를 구분해야함.
    public int Energy
    {
        get
        {
            return _energy;
        }
        set
        {
            if (value > 0)
            {
                _energy += value;
            }
            else if (value < 0)
            {
                TryUseEnergy(value);
            }
        }
    }
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

            ChangeCurrentWeapon(value);
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
            ChangeCurrentClothes(value);
        }
    }

    /// <summary>
    /// CurrentStage의 경우 대입하는 값에 상관 없이 무조건 1 증가한다.
    /// </summary>
    public int CurrentStage
    {
        get
        {
            return _currentStage;
        }
        set
        {
            _currentStage++;
            // maxStage를 넘어가면 0으로 돌리거나, 랜덤 스테이지로 가거나.
        }
    }
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
    public IReadOnlyDictionary<int, bool> EquipmentUnLockDic => _equipmentUnLockDic;

    public int UnLockedWeaponCount => _unLockedWeaponCount;
    public int UnLockedClothesCount => _unLockedClothesCount;

    // playerCharacter
    public int Attack
    {
        get
        {
            // 기본
            int result = _defaultAttack;
            // 무기
            result += _currentWeapon.AdditionalAtt;
            // 의상 (라희 추가)
            result += _currentClothes.AdditionalAtt;
            // 업그레이드
            result += (CSOManager.Instance[CDataArraySO.EDataType.AbilityData][0] as CAbilityDataSO).Val * _currentUpgradeLevel[0];
            // 재능
            var talentAtt = CSOManager.Instance[CDataArraySO.EDataType.TalentData][4] as CTalentDataSO;
            int talentAttLevel = _currentTalentLevel[4];
            if (talentAttLevel > 0)
            {
                result += talentAtt.Basic + talentAtt.Volume * (talentAttLevel - 1);
            }
            // 배율
            float ratio = 1f;
            if (_currentWeapon.BonusType == CEquipmentDataSO.EBonusType.Attak)
            {
                ratio += 0.01f * (_currentWeapon.BonusAmount);
            }
            // 의상 배율 (라희 추가)
            if (_currentClothes.BonusType == CEquipmentDataSO.EBonusType.Attak)
            {
                ratio += 0.01f * _currentClothes.BonusAmount;
            }


            ratio += 0.005f * _unLockedWeaponCount;
            return (int)(result * ratio);
        }
    }
    public float CriticalRate
    {
        get
        {
            float result = _defaultCriticalRate;
            // 재능
            var talentCri = CSOManager.Instance[CDataArraySO.EDataType.TalentData][2] as CTalentDataSO;
            int talentCriLevel = _currentTalentLevel[2];
            if (talentCriLevel > 0)
            {
                result += 0.01f * (talentCri.Basic + talentCri.Volume * (talentCriLevel - 1));
            }
            return result;
        }
    }
    // 치명타시 추가 공격력
    public int AdditionalCriticalAttack
    {
        get
        {
            int result = (int)(Attack * _defaultCriticalAttackRate);
            // 재능
            var talentCri = CSOManager.Instance[CDataArraySO.EDataType.TalentData][6] as CTalentDataSO;
            int talentCriLevel = _currentTalentLevel[6];
            if (talentCriLevel > 0)
            {
                result += talentCri.Basic + talentCri.Volume * (talentCriLevel - 1);
            }
            return result;
        }
    }

    // 백어택시 추가 공격력
    public int AdditionalBackAttack
    {
        get
        {
            int result = (int)(Attack * _defaultBackAttackRate);
            // 재능
            var talentCri = CSOManager.Instance[CDataArraySO.EDataType.TalentData][6] as CTalentDataSO;
            int talentCriLevel = _currentTalentLevel[6];
            if (talentCriLevel > 0)
            {
                result += talentCri.Basic + talentCri.Volume * (talentCriLevel - 1);
            }
            return result;
        }
    }
    public int Defence
    {
        get
        {
            // 기본
            int result = _defaultDefence;
            // 재능
            var talentAtt = CSOManager.Instance[CDataArraySO.EDataType.TalentData][0] as CTalentDataSO;
            int talentAttLevel = _currentTalentLevel[0];
            if (talentAttLevel > 0)
            {
                result += talentAtt.Basic + talentAtt.Volume * (talentAttLevel - 1);
            }
            return result;
        }
    }
    public int HP
    {
        get
        {
            // 기본
            int result = _defaultHp;
            // 업그레이드
            result += (CSOManager.Instance[CDataArraySO.EDataType.AbilityData][1] as CAbilityDataSO).Val * _currentUpgradeLevel[1];
            // 재능
            var talentAtt = CSOManager.Instance[CDataArraySO.EDataType.TalentData][5] as CTalentDataSO;
            int talentAttLevel = _currentTalentLevel[5];
            if (talentAttLevel > 0)
            {
                result += talentAtt.Basic + talentAtt.Volume * (talentAttLevel - 1);
            }
            // 배율
            float ratio = 1f;
            if (_currentWeapon.BonusType == CEquipmentDataSO.EBonusType.Health)
            {
                ratio += 0.01f * (_currentWeapon.BonusAmount);
            }
            // 의상 배율 (라희 추가)
            if (_currentClothes.BonusType == CEquipmentDataSO.EBonusType.Health)
            {
                ratio += 0.01f * _currentClothes.BonusAmount;
            }

            ratio += 0.005f * _unLockedWeaponCount;
            ratio += 0.005f * _unLockedClothesCount;
            return (int)(result * ratio);
        }
    }

    public int Recovery
    {
        get
        {
            // 기본
            int result = _defaultRecovery;
            // 재능
            var talentAtt = CSOManager.Instance[CDataArraySO.EDataType.TalentData][1] as CTalentDataSO;
            int talentAttLevel = _currentTalentLevel[1];
            if (talentAttLevel > 0)
            {
                result += talentAtt.Basic + talentAtt.Volume * (talentAttLevel - 1);
            }
            return result;
        }
    }
    public float MoveSpeed
    {
        get
        {
            // 기본
            float result = _defaultMoveSpeed;
            // 업그레이드
            result += (CSOManager.Instance[CDataArraySO.EDataType.AbilityData][2] as CAbilityDataSO).Val * _currentUpgradeLevel[2];
            // 재능
            var talentAtt = CSOManager.Instance[CDataArraySO.EDataType.TalentData][7] as CTalentDataSO;
            int talentAttLevel = _currentTalentLevel[7];
            if (talentAttLevel > 0)
            {
                result += 0.01f * (talentAtt.Basic + talentAtt.Volume * (talentAttLevel - 1));
            }
            // 배율
            float ratio = 1f;
            if (_currentWeapon.BonusType == CEquipmentDataSO.EBonusType.MoveSpeed)
            {
                ratio += 0.01f * (_currentWeapon.BonusAmount);
            }

            // 의상 배율 (라희 추가)
            if (_currentClothes.BonusType == CEquipmentDataSO.EBonusType.MoveSpeed)
            {
                ratio += 0.01f * _currentClothes.BonusAmount;
            }

            ratio += 0.005f * _unLockedClothesCount;

            return (int)(result * ratio);
        }
    }
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

        // 저장된 무기,의상 ID로 실제 무기 데이터(SO) 가져와서 연결 (라희 추가)
        _currentWeapon = CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][_currentWeaponID] as CEquipmentDataSO;
        _currentClothes = CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][_currentClothesID] as CEquipmentDataSO;
    }

    void Update()
    {

    }

    public void ChangeCurrentEquipment(int id)
    {
        if (id < 1000)
        {
            ChangeCurrentWeapon(id);
        }
        else
        {
            ChangeCurrentClothes(id);
        }
    }
    private void ChangeCurrentClothes(int id)
    {
        if (_currentClothesID != id)
        {
            _currentClothesID = id;
            _currentClothes = CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][id] as CEquipmentDataSO;

            OnStatChanged?.Invoke();  // 라희 추가
        }
    }

    private void ChangeCurrentWeapon(int id)
    {
        if (_currentWeaponID != id)
        {
            _currentWeaponID = id;
            _currentWeapon = CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][id] as CEquipmentDataSO;

            OnStatChanged?.Invoke(); // 라희 추가
        }
    }
    public void UnLockEquipmentDic(int ID)
    {
        if (_equipmentUnLockDic.ContainsKey(ID))
        {
            _equipmentUnLockDic[ID] = true;
            if (ID < 1000)
            {
                _unLockedWeaponCount++;
            }
            else
            {
                _unLockedClothesCount++;
            }
        }
        else
        {
            Debug.LogWarning($"사전에 등록되지 않은 key값에 접근. key = {ID}");
        }
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
        if (_currentGem >= gem)
        {
            _currentGem -= gem;
            return true;
        }
        return false;
    }


    public void MakeSaveData()
    {
        if (_data == null)
            _data = new PlayerSaveData();

        _data.MaxGem = _maxGem;
        _data.CurrentGem = _currentGem;
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

        _maxGem = _data.MaxGem;
        _currentGem = _data.CurrentGem;
        _energy = _data.Energy;

        CurrentWeaponID = _data.CurrentWeaponID;
        CurrentClothesID = _data.CurrentClothesID;

        _currentStage = _data.CurrentStage;

        _currentUpgradeLevel = _data.CurrentUpgradeLevel;
        _currentTalentLevel = _data.CurrentTalentLevel;

        _equipmentUnLockDic.ArrayToDic(_data.EquipmentDicID, _data.EquipmentDicValue);
        _unLockedWeaponCount = 0;
        _unLockedClothesCount = 0;
        foreach ((int key, bool value) in _equipmentUnLockDic)
        {
            if (value)
            {
                if (key < 1000)
                {
                    _unLockedWeaponCount++;
                }
                else
                {
                    _unLockedClothesCount++;
                }
            }
        }
    }

    public void InitSaveData()
    {
        if (_data != null)
        {
            _data = null;
        }
        _data = new PlayerSaveData();
    }
}
