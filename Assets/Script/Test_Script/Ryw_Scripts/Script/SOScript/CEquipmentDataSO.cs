using System;
using UnityEngine;


#region CEquipmentDataSO
/*
▶ 작성자 류연우
// 3가지 스탯을 하나로 묶는걸 고려.
// 그냥 따로 더하지 말고 + 연산자를 오버리이딩해 클래스를 더해버리도록..


ID,타입,추가 공격력,공격력 배율,체력 배율,이동 속도 배율,이미지
*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Equipment Data (SO)", fileName = "EquipmentDataSO_")]
public class CEquipmentDataSO : ScriptableObject, ICSVData
{
    static readonly string NAME = "EquipmentData";

    public enum EEquipmentType
    {
        Dagger,
        Spear,
        Sword,
        Skin
    }

    public enum EBonusType
    {
        Attak,
        Health,
        MoveSpeed
    }

    #region 인스펙터
    [SerializeField] private int _ID = 0;
    [SerializeField] private EEquipmentType _type = EEquipmentType.Dagger;
    [SerializeField] private int _additionalAtt = 0;
    [SerializeField] private float _additionalAttackRatio = 0;
    [SerializeField] private float _additionalHealthRatio = 0;
    [SerializeField] private float _additionalSpeedRatio = 3;
    [SerializeField] private Sprite _image;
    [SerializeField] private string _objectName;
    // pre...
    #endregion

    #region 내부 변수
    private (float value, EBonusType type)[] bonuses;
    private (float value, EBonusType type)? maxBonus;
    #endregion

    #region 프로퍼티
    public int ID => _ID;
    public EEquipmentType Type => _type;
    public int AdditionalAtt => _additionalAtt;
    public float BonusAmount => Math.Max(_additionalAttackRatio, Math.Max(_additionalHealthRatio, _additionalSpeedRatio));
    // BonusAmount가 가장 큰 type을 반환.
    public EBonusType BonusType
    {
        get
        {
            if (bonuses == null)
            {
                bonuses = new (float value, EBonusType type)[]
                {
                    (_additionalAttackRatio, EBonusType.Attak),
                    (_additionalHealthRatio, EBonusType.Health),
                    (_additionalSpeedRatio, EBonusType.MoveSpeed)
                };
            }

            if (maxBonus == null)
            {
                maxBonus = bonuses[0];
                for (int i = 1; i < bonuses.Length; i++)
                {
                    if (bonuses[i].value > maxBonus.Value.value)
                    {
                        maxBonus = bonuses[i];
                    }
                }
            }

            return maxBonus.Value.type;
        }
    }
    public float AdditionalAttackRatio => _additionalAttackRatio;
    public float AdditionalHealthRatio => _additionalHealthRatio;
    public float AdditionalSpeedRatio => _additionalSpeedRatio;
    public Sprite Image => _image;

    public string OjbectName => _objectName;
    #endregion


    public string ParseData(string data)
    {
        string[] dataArr = data.Split(",");

        _ID = int.Parse(dataArr[0]);
        _type = (EEquipmentType)Enum.Parse(typeof(EEquipmentType), dataArr[1]);
        _additionalAtt = int.Parse(dataArr[2]);
        _additionalAttackRatio = float.Parse(dataArr[3]);
        _additionalHealthRatio = float.Parse(dataArr[4]);
        _additionalSpeedRatio = float.Parse(dataArr[5]);
        _image = _image.ParseData(dataArr[6]);
        _objectName = dataArr[7];

        bonuses = null;
        maxBonus = null;

        return CGSSLoader.SOSavePath(NAME) + $"/{NAME}SO_{_ID}.asset";
    }
}
