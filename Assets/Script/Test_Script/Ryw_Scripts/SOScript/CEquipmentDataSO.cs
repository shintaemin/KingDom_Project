using UnityEngine;


#region CEquipmentDataSO
/*
▶ 작성자 류연우
// 3가지 스탯을 하나로 묶는걸 고려.
// 그냥 따로 더하지 말고 + 연산자를 오버리이딩해 클래스를 더해버리도록..
*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Equipment Data (SO)", fileName = "EquipmentDataSO_")]
public class CEquipmentDataSO : ScriptableObject
{
    public enum EEquipmentType
    {
        Dagger,
        Spear,
        Sword,
        Skin
    }
    #region 인스펙터
    [SerializeField] private int _ID = 0;
    [SerializeField] private EEquipmentType _type = EEquipmentType.Dagger;
    [SerializeField] private int _additionalAtt = 0;
    [SerializeField] private float _additionalAttackRatio = 0;
    [SerializeField] private float _additionalHealthRatio = 0;
    [SerializeField] private float _additionalSpeedRatio = 3;
    [SerializeField] private Texture2D _image;
    #endregion

    #region 프로퍼티
    public int ID => _ID;
    public EEquipmentType Type => _type;
    public int AdditionalAtt => _additionalAtt;
    public float AdditionalAttackRatio => _additionalAttackRatio;
    public float AdditionalHealthRatio => _additionalHealthRatio;
    public float AdditionalSpeedRatio => _additionalSpeedRatio;
    public Texture2D Image => _image;
    #endregion
}
