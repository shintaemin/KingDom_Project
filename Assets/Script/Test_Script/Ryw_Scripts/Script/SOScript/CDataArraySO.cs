using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


#region CDataArraySO
/*
▶ 작성자 류연우

가능하면 매니저를 통해 사용할것.
아마도, 인스턴스를 생성하지 않고 직접 사용했을때, 메모리 문제가 생기지 않을 것이다. 아마도.

*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Data Array (SO)", fileName = "DataArraySO_")]
public class CDataArraySO : ScriptableObject
{
    static readonly string NAME = "DataArraySO";
    public enum EDataType
    {
        EquipmentData,
        TalentData,
        MissionData,
        AbilityData
    }

    #region 인스펙터
    [SerializeField] private List<CTalentDataSO> _talentDataArr;
    [SerializeField] private List<CMissionDataSO> _missionDataArr;
    [SerializeField] private List<CAbilityDataSO> _abilityDataArr;
    [SerializeField] private List<CEquipmentDataSO> _equipmentDataArr;
    #endregion

    public ICSVData FUnc()
    {
        return _talentDataArr[0];
    }

    #region 내부 변수
    private Dictionary<int, ICSVData> _talentDataDic;
    private Dictionary<int, ICSVData> _missionDataDic;
    private Dictionary<int, ICSVData> _abilityDataDic;
    private Dictionary<int, ICSVData> _equipmentDataDic;

    //public IReadOnlyDictionary<int, CTalentDataSO> TalentDataDic => _talentDataArr.ToDictionary(data => data.ID);
    public IReadOnlyDictionary<int, ICSVData> TalentDataDic => _talentDataDic ??= InitDataDic(_talentDataArr);
    public IReadOnlyDictionary<int, ICSVData> MissionDataDic => _missionDataDic ??= InitDataDic(_missionDataArr);
    public IReadOnlyDictionary<int, ICSVData> AbilityDataDic => _abilityDataDic ??= InitDataDic(_abilityDataArr);
    public IReadOnlyDictionary<int, ICSVData> EquipmentDataDic => _equipmentDataDic ??= InitDataDic(_equipmentDataArr);

    //public IReadOnlyDictionary<EDataType, IReadOnlyDictionary<int, ICSVData>> DataDic => _dataDic ??= InitDataDic();

    //private object InitDataDic()
    //{

    //}
    #endregion


    public IReadOnlyDictionary<int, ICSVData> this[EDataType dataType] => dataType switch
    {
        EDataType.TalentData => TalentDataDic,
        EDataType.AbilityData => AbilityDataDic,
        EDataType.MissionData => MissionDataDic,
        EDataType.EquipmentData => EquipmentDataDic,
        _ => null
    };

    private Dictionary<int, ICSVData> InitDataDic<T>(List<T> list) where T : ICSVData
    {
        var dic = new Dictionary<int, ICSVData>();
        foreach (var item in list)
        {
            if (item == null) continue;

            if (!dic.TryAdd(item.ID, item))
            {
                Debug.LogError($"중복된 ID 발견: {item.ID} (타입: {typeof(T).Name})");
            }
        }
        return dic;
    }

    public string SetData()
    {
#if UNITY_EDITOR
        Debug.Log("Set DataArraySO");

        SetSOAsset(ref _talentDataArr);
        SetSOAsset(ref _missionDataArr);
        SetSOAsset(ref _equipmentDataArr);
        SetSOAsset(ref _abilityDataArr);
#endif
        return Path.Combine(CGSSLoader.SO_PATH, NAME+".asset");
    }

    private void SetSOAsset<T>(ref List<T> soList) where T : UnityEngine.Object, ICSVData
    {
#if UNITY_EDITOR
        string[] list = AssetDatabase.FindAssets($"t:{typeof(T).Name}");


        soList ??= new List<T>();

        for (int j = 0; j < list.Length; j++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(list[j]);
            soList.Add(AssetDatabase.LoadAssetAtPath<T>(assetPath));
        }

        Debug.Log($"{typeof(T).Name} : {list.Length}");
#endif
    }
}
