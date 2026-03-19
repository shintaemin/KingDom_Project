using UnityEngine;


#region CAbilityDataSO
/*
▶ 작성자 류연우
*/
#endregion

[CreateAssetMenu(menuName = "Create SO/Data/Ability Data (SO)", fileName = "AbilityDataSO_")]
public class CAbilityDataSO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private int _ID = 0;
    [SerializeField] private int _val = 20;
    [SerializeField] private int[] _priceArr = { 300, 600, 800, 870, 930, 990, 1050, 1110, 1170, 1240, 1310, 1380, 1450, 1520, 1620, 1720, 1820, 1920, 2020, 2130, 2240, 2350, 2460, 2570}; //...
    [SerializeField] private Texture2D[] _iconArr;
    #endregion

    #region 프로퍼티
    public int ID => _ID;
    public int Val => _val;
    public int[] PriceArr => _priceArr;
    public int MaxPriceLevel => _priceArr.Length;
    public Texture2D[] IconArr => _iconArr;
    #endregion
}
