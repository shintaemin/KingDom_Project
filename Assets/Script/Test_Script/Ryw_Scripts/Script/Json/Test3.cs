using UnityEngine;


#region Test3
/*

*/
#endregion

public class Test3 : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [SerializeField] private float floatData;

    public string stringData;
    #endregion

    #region 내부 변수

    #endregion
    public void Start()
    {
        CJsonManager.Instance.Add("test3", this, typeof(Test3));
    }
}
