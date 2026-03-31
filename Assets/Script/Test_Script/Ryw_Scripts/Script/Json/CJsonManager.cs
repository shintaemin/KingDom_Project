using System.Collections.Generic;
using System.IO;
using UnityEngine;


#region JsonTest
/*
▶작성자 류연우

직렬화가 가능한 정보만 가능하다.
    인스펙터에 노출되는 정보만 가능하다.


이를 위해 따로 클래스를 구현하거나, 자기 자신에 [System.Serializable]를 붙여야 할 것.
*/
#endregion

// public 만 json이 읽어올 수 있다.
[System.Serializable]
public class JsonDataTest : IJsonData
{
    public string stringData;
    public int intData;
    public float floatData;
    public bool boolData;
    public List<string> inventory = new List<string>();

    [SerializeField] private int ID;

    public JsonDataTest()
    {
        // 이 랜덤은 유니티의 랜덤인데, 이 경우 직렬화나 생성자에서 사용하면 오류가 생길 수 있다고.ㅇ ㅓㅈ
        // 그러니까 system의 랜덤을 사용하로
        //ID = Random.Range(0, 10000);
        System.Random _rand = new System.Random();
        ID = new System.Random().Next(0, 10001);
    }
}

public class CJsonManager : MonoBehaviour
{
    #region 인스펙터

    #endregion

    #region 내부 변수
    public static CJsonManager Instance;

    [Header("디버그용 클래스")]
    public JsonDataTest TestData;
    #endregion

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance != null && Instance != this");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData(TestData, "saveData");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData(out TestData, "saveData");
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void SaveData<T>(T data, string pileName) where T : IJsonData
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, $"{pileName}.json");

        File.WriteAllText(path, json);

        Debug.Log($"저장된 경로 : {path} \n 저장된 내용 : {json}");
    }

    private void LoadData<T>(out T data, string pileName) where T : class, IJsonData
    {
        string path = Path.Combine(Application.persistentDataPath, $"{pileName}.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);

            Debug.Log($"불러오기 경로 : {path} \n 불러온 내용 : {json}");
        }
        else
        {
            Debug.Log("없음");
            data = null;
        }
    }
}
