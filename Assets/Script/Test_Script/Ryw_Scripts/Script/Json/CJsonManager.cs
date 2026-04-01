using System.Collections.Generic;
using System.IO;
using UnityEngine;


#region JsonTest
/*
▶작성자 류연우

직렬화가 가능한 정보만 가능하다.
    인스펙터에 노출되는 정보만 가능하다.
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

    public void Add(string key, IJsonData data, System.Type type)
    {
        if (SavaDataDictionary == null)
            SavaDataDictionary = new Dictionary<string, (IJsonData, System.Type)>();

        if (!SavaDataDictionary.ContainsKey(key))
        {
            //TryAdd
            SavaDataDictionary.Add(key, (data, type));
        }
        else
        {
            Debug.LogWarning($"{key}는 이미 있는 키. 같은 오브젝트를 넣고 있거나, 같은 key를 사용중인듯.");
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

    private void SaveData<T>(T data, string pileName) where T : IJsonData
    {
        string json = JsonUtility.ToJson(data, true);
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
            LoadData(out value.Item1, key);

            //switch (type)
            //{
                //case :
                    //LoadData<>(out value.Item1, key);

            //}


            if (value.Item1 != null)
            {
                SavaDataDictionary[key] = value;
            }
        }
    }

    private void LoadData<T>(out T data, string pileName) where T : class, IJsonData
    {
        string path = Path.Combine(Application.persistentDataPath, $"{pileName}.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = null;
            //data = JsonUtility.FromJson<T>(json);

            Debug.Log($"불러오기 경로 : {path} \n 불러온 내용 : {json}");
        }
        else
        {
            Debug.Log("없음");
            data = null;
        }
    }
}
