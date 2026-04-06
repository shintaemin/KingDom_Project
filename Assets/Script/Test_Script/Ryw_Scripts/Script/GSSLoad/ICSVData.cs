

#region ICVSData
/*
▶ 작성자 류연우

CGSSLoader에서 읽어온 데이터를 파싱할 수 있는 객체를 위한 인터페이스?
*/
#endregion

public interface ICSVData
{
    public string ParseData(string data);

    public int ID { get; }
}
