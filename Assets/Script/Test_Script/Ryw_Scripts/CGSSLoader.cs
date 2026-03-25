using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


#region CGSSLoader
/*
▶ 작성자 류연우
런타임에 작동할 녀석이 아닙니다.
미리 SO를 만들어줄 녀석입니다.

이 클래스를 통해 만들어진 SO를 사용할것.

    참고
https://data-pandora.tistory.com/entry/Unity-%EA%B5%AC%EA%B8%80-%EC%8A%A4%ED%94%84%EB%A0%88%EB%93%9C%EC%8B%9C%ED%8A%B8-%EC%97%B0%EB%8F%99
*/
#endregion

public class CGSSLoader : MonoBehaviour
{
    private enum ESheetType
    {
        EquipmentData,

    }
    #region 인스펙터
    public bool PrintData = true;
    #endregion

    #region 내부 변수
    const string EXTRA_URL = "/export?format=";
    const string LOAD_TYPE = "csv";
    const string URL = "https://docs.google.com/spreadsheets/d/1wx7tsBCYFjxJkCGeNdoklLhEJamttKUGMLcJNghr1wc";
    // sheet 페이지별 gid의 배열.
    static readonly string[] EXTRA_LOAD = new string[] { "", "&gid=1949371489#gid=1949371489" };
    #endregion

    void Awake()
    {

    }

    void Start()
    {
        StartCoroutine(LoadFromURL(ESheetType.EquipmentData));
    }
    IEnumerator LoadFromURL(ESheetType type)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + EXTRA_URL + LOAD_TYPE + EXTRA_LOAD[(int)type]);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        string[] dataArr = data.Split("\n");
        // 문자열을 string.Split('\n')로 분리해 하나의 객체로 만든다.
        // 분리된 각 줄을 다시 string.Split(',')로 분리해 의미있는 정보로 만든다.
        // 미리 정해둔 규약에 따라 정리한다.
        // 야호! 사용해.

        // 0,1은 사용자 편의를 위한 수치가 들어감. 2번부터 데이터임.
        for (int i = 2; i < dataArr.Length; i++)
        {
            CEquipmentDataSO instance = ScriptableObject.CreateInstance<CEquipmentDataSO>();

            instance.ParsingData(dataArr[i]);
        }

        if (PrintData)
        {
            foreach (string s in dataArr)
            {
                print(s);
            }
        }
    }

    void Update()
    {

    }
}
