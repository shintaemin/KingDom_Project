using UnityEngine;

#region 플레이어 데이터 SO
/*
 ▶ 할일
  - 플레이어 데이터를 관리
*/
#endregion


[CreateAssetMenu(menuName = "Create SO/Data/Talent Data (SO)", fileName = "TalentDataSO_")]
public class CTalentDataSO : ScriptableObject
{
    // 방어력을 예시로
    #region 인스펙터
    [SerializeField] private string _name = "방어력";
    [SerializeField] private string _information = "방어력 {}상승";
    [SerializeField] private int _basic = 30;
    [SerializeField] private int _volume = 10;
    //[SerializeField] private 그림? _icon;
    #endregion

    #region 외부 호출 함수
    public string Name => _name;
    public string Information => _information;
    public int Basic => _basic;
    public int Volume => _volume;
    #endregion


}
