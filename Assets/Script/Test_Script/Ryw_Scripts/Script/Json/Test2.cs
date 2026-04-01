using UnityEngine;


#region Test2
/*

*/
#endregion

public class Test2 : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [SerializeField] private float floatData;

    public string stringData;
    #endregion

    #region 내부 변수

    #endregion
    public void Start()
    {
        CJsonManager.Instance.Add("test2", this, typeof(Test2));
    }
}
