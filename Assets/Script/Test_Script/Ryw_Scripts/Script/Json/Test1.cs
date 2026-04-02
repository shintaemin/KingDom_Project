using UnityEngine;


#region Test1
/*
▶작성자 류연우

CJsonManager 사용을 위한 예시.
*/
#endregion

[System.Serializable]
public class MyData
{
    public float floatData;

    public string stringData;
}


public class Test1 : MonoBehaviour, IJsonData
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
        CJsonManager.Instance.Add("test1", this, typeof(MyData));
    }

    public void MakeSaveData()
    {
        // 실질적으로는 _data의 값을 저장하기 때문에 없으면 곤란하다.
        if (_data == null)
            _data = new MyData();

        _data.floatData = _floatData;
        _data.stringData = _stringData;
    }

    public void LoadSaveData()
    {
        // .json에서 _data로 불러온 후 동작할 함수이다. 없으면 뭔가 잘못된거다.
        if (_data.IsNull("_data"))
            return;

        _floatData = _data.floatData;
        _stringData = _data.stringData;
    }
}
