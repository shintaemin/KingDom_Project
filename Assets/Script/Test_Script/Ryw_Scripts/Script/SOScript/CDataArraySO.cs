using System.Collections.Generic;
using UnityEngine;


#region CDataArraySO
/*
▶ 작성자 류연우

*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Data Array (SO)", fileName = "DataArraySO_")]
public class CDataArraySO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private List<CTalentDataSO> _talentDataArr;
    [SerializeField] private List<CMissionDataSO> _missionDataArr;
    [SerializeField] private List<CAbilityDataSO> _abilityDataArr;
    [SerializeField] private List<CEquipmentDataSO> _equipmentDataArr;
    #endregion

    #region 내부 변수
    public IReadOnlyList<CTalentDataSO> TalentDataArr => _talentDataArr;
    public IReadOnlyList<CMissionDataSO>MissionDataArr => _missionDataArr;
    public IReadOnlyList<CAbilityDataSO> AbilityDataArr => _abilityDataArr;
    public IReadOnlyList<CEquipmentDataSO> EquipmentDataSOs => _equipmentDataArr;
    #endregion
}
