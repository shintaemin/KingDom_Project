using System.Collections.Generic;
using UnityEngine;


#region CDataManager
/*
▶ 작성자 류연우

총 3가지 방법으로 사용이 가능하다.
1. enum 타입과 ID 입력, 형변환
var tmp = CDataManager.Instance[CDataArraySO.EDataType.TalentData][0] as CTalentDataSO;

2. 제네릭 타입 입력, enum 타입과 ID 입력
var tmp2 = CDataManager.Instance.SuchData<CTalentDataSO>(CDataArraySO.EDataType.TalentData, 0);

3. 미리 변수 선언 후 인자로 넘김, ID 입력. 이 경우 var 사용 불가능.
CTalentDataSO tmp3;
CDataManager.Instance.SuchData(out tmp3, 0);
or
if(CDataManager.Instance.SuchData(out CTalentDataSO tmp3, 0))
{

}

결국 3가지 방법 전부 실제 클래스의 타입을 알아야 사용이 가능하다.

※ 번외로,
매니저에서 정해둔 매소드를 사용하기 싫을 때, 혹 딕셔너리 그 자체가 필요한 경우
직접 사용을 위해 ReadOnlyDictionary를 public 으로 만듦.
*/
#endregion

public class CDataManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CDataArraySO _dataArraySO;
    #endregion

    #region 내부 변수
    public static CDataManager Instance;

    // 비추
    public CDataArraySO DataArraySO => _dataArraySO;
    #endregion

    // 1.
    public IReadOnlyDictionary<int, ICSVData> this[CDataArraySO.EDataType dataType] => _dataArraySO[dataType];

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance != null && Instance != this");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _dataArraySO.IsNull("_dataArraySO");

        DontDestroyOnLoad(this.gameObject);
    }


    // 2.
    public T SuchData<T>(CDataArraySO.EDataType dataType, int ID) where T : class, ICSVData
    {
        switch (dataType)
        {
            case CDataArraySO.EDataType.TalentData:
                return (T)_dataArraySO.TalentDataDic[ID];
            case CDataArraySO.EDataType.MissionData:
                return (T)_dataArraySO.MissionDataDic[ID];
            case CDataArraySO.EDataType.AbilityData:
                return (T)_dataArraySO.AbilityDataDic[ID];
            case CDataArraySO.EDataType.EquipmentData:
                return (T)_dataArraySO.EquipmentDataDic[ID];
        }
        return null;
    }

    // 3.
    public bool SuchData<T>(out T data, int ID) where T : class, ICSVData
    {
        System.Type type = typeof(T);

        if (type == typeof(CTalentDataSO))
        {
            data = (T)_dataArraySO.TalentDataDic[ID];
            return true;
        }
        else if (type == typeof(CMissionDataSO))
        {
            data = (T)_dataArraySO.MissionDataDic[ID];
            return true;
        }
        else if (type == typeof(CAbilityDataSO))
        {
            data = (T)_dataArraySO.AbilityDataDic[ID];
            return true;
        }
        else if (type == typeof(CEquipmentDataSO))
        {
            data = (T)_dataArraySO.EquipmentDataDic[ID];
            return true;
        }

        data = null;
        return false;
    }

    void Start()
    {

    }


    void Update()
    {

    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
