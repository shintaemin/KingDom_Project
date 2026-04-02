using UnityEngine;


#region Test3
/*

*/
#endregion

public class Test3 : MonoBehaviour, IJsonData
{
    #region 인스펙터
    [SerializeField] private float _floatData;

    public string _stringData;

    #endregion

    #region 내부 변수
    private MyData _data;
    public object SaveData { get => _data; set => _data = (MyData)value; }
    #endregion
    public void Start()
    {
        CJsonManager.Instance.Add("test3", this, typeof(MyData));
    }

    public void MakeSaveData()
    {
        if (_data == null)
            _data = new MyData();

        _data.floatData = _floatData;
        _data.stringData = _stringData;
    }

    public void LoadSaveData()
    {
        if (_data == null)
            _data = new MyData();

        _floatData = _data.floatData;
        _stringData = _data.stringData;
    }
}
