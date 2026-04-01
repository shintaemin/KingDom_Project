using System.Collections.Generic;
using System.IO;
using UnityEngine;


#region JsonTest
/*
▶작성자 류연우

데이터를 저장하고 싶은 클래스에 IJsonData를 상속받고 해당 멤버들을 구현해주면 된다.
그 후 CJsonManager.Instance.Add("test1", this, typeof(MyData)); 와 같은 코드를 start()에 넣어주면 된다.

*/
#endregion

public class CJsonManager : MonoBehaviour
{
    #region 인스펙터

    #endregion

    #region 내부 변수
    public static CJsonManager Instance;

    public Dictionary<string, (IJsonData, System.Type)> SavaDataDictionary;
    //public Dictionary<string, IJsonData> SavaDataDictionary;
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
            SaveAll();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadAll();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Add(string fileName, IJsonData data, System.Type type)
    {
        if (SavaDataDictionary == null)
            SavaDataDictionary = new Dictionary<string, (IJsonData, System.Type)>();

        if (!SavaDataDictionary.ContainsKey(fileName))
        {
            //TryAdd
            SavaDataDictionary.Add(fileName, (data, type));
        }
        else
        {
            Debug.LogWarning($"{fileName}는 이미 있는 키. 같은 오브젝트를 넣고 있거나, 같은 key를 사용중인듯.");
        }
    }

    public void SaveAll()
    {
        if (SavaDataDictionary == null)
            return;

        foreach (var data in SavaDataDictionary)
        {
            var value = data.Value;
            var key = data.Key;
            SaveData(value.Item1, key);
        }
    }

    private void SaveData(IJsonData data, string pileName)
    {
        data.MakeSaveData();
        string json = JsonUtility.ToJson(data.SaveData, true);
        string path = Path.Combine(Application.persistentDataPath, $"{pileName}.json");

        File.WriteAllText(path, json);

        Debug.Log($"저장된 경로 : {path} \n 저장된 내용 : {json}");
    }

    public void LoadAll()
    {
        if (SavaDataDictionary == null)
            return;

        foreach (var data in SavaDataDictionary)
        {
            var key = data.Key;
            var value = data.Value;
            System.Type type = value.Item2;

            LoadData(key, value.Item1, type);
        }
    }

    private void LoadData (string pileName, IJsonData data, System.Type type)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{pileName}.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data.SaveData = JsonUtility.FromJson(json, type);

            data.LoadSaveData();

            Debug.Log($"불러오기 경로 : {path} \n 불러온 내용 : {json}");
        }
        else
        {
            Debug.Log("없음");
        }

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
