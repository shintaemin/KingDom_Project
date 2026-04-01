using UnityEngine;


#region Test1
/*

*/
#endregion

public class Test1 : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [SerializeField] private float floatData;

    public string stringData;
    #endregion

    #region 내부 변수

    #endregion

    public void Start()
    {
        CJsonManager.Instance.Add("test1", this, typeof(Test1));
    }
}
