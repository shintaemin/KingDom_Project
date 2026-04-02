#region IJsonData
/*
▶작성자 류연우

SaveData
MonoBehaviour를 상속받는 객체의 경우 new()로 생성이 불가능해 역직렬화가 불가능하다.
따라서 json 변환을 위한 클래스를 따로 만들어야하는데, 그 클래스를 가리킬 프로퍼티이다.

MakeSaveData
CJsonManager에서 정보를 저장하기 전에 SaveData의 값을 만들어주는 함수. 직접 구현해야한다.

LoadSaveData
SaveData의 값을 역직렬화 후 해당 클래스의 정보를 자신에게 적용해주는 함수. 직접 구현해야한다.


※ 데이터 클래스의 경우 세이브 파일 초기화 기능을 위해 기본 값을 초기값이라고 생각하고 할당해주길 바람.
아니라면 이것도 스프레드 시트를 만든다.

예시
[System.Serializable]
public class MyData
{
    ...
}

public class Test1 : MonoBehaviour, IJsonData
{
    ...

    private MyData _data;
    
    public object SaveData { get => _data; set => _data = (MyData)value; }

    public void MakeSaveData()
    {
        // 실질적으로는 _data의 값을 저장하기 때문에 없으면 곤란하다.
        if (_data == null)
            _data = new MyData();
        ...
    }

    public void LoadSaveData()
    {
        // .json에서 _data로 불러온 후 동작할 함수이다. 없으면 뭔가 잘못된거다.
        if (_data.IsNull("_data"))
            return;
        ...
    }
}
*/
#endregion

public interface IJsonData
{
    object SaveData { get; set;}
    public void MakeSaveData();
    public void LoadSaveData();
}
