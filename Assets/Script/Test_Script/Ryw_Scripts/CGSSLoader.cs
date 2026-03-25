using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


#region CGSSLoader
/*
▶ 작성자 류연우

런타임에 작동할 녀석이 아닙니다.
미리 SO를 만들어줄 녀석입니다.

프로젝트 빌드 전 별도의 씬에서 SO 생성을 위해 사용할 클래스이므로 
아마 느려도 상관 없을것...

이 클래스를 통해 만들어진 SO를 사용할것.

SO의 값을 변경하지 말고 스프레드시트의 값을 수정할것.

인스펙터에서 새로 생성할 시트를 골라 씬을 실행시키면 된다.

    참고
https://data-pandora.tistory.com/entry/Unity-%EA%B5%AC%EA%B8%80-%EC%8A%A4%ED%94%84%EB%A0%88%EB%93%9C%EC%8B%9C%ED%8A%B8-%EC%97%B0%EB%8F%99
*/
#endregion

public class CGSSLoader : MonoBehaviour
{
    private enum ESheetType
    {
        EquipmentData,
        TalentData,
        MissionData,
        AbilityData,
        Count
    }

    [Flags]
    public enum ECreateFlag
    {
        EquipmentData = 1 << 0,
        TalentData = 1 << 1,
        MissionData = 1 << 2,
        AbilityData = 1 << 3
    }

    #region 인스펙터
    public bool PrintData = true;
    public ECreateFlag CreateFlag = 0;
    #endregion

    #region 내부 변수
    const string URL = "https://docs.google.com/spreadsheets/d/1wx7tsBCYFjxJkCGeNdoklLhEJamttKUGMLcJNghr1wc";
    const string EXTRA_URL = "/export?format=";
    const string LOAD_TYPE = "csv";
    // sheet 페이지별 gid의 배열.
    static readonly string[] EXTRA_LOAD = new string[] { "", "&gid=214534590#gid=214534590", "&gid=299399325#gid=299399325", "&gid=2096374625#gid=2096374625" };
    #endregion

    void Awake()
    {
    }

    void Start()
    {
        for (int i = 0; i < (int)ESheetType.Count; i++)
        {
            if ((CreateFlag & (ECreateFlag)(1 << i)) != 0)
                StartCoroutine(LoadFromURL((ESheetType)i));
        }
    }

    IEnumerator LoadFromURL(ESheetType type)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + EXTRA_URL + LOAD_TYPE + EXTRA_LOAD[(int)type]);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        string[] dataArr = data.Split("\n");

        // 0,1은 사용자 편의를 위한 수치가 들어감. 2번부터 데이터임.
        for (int i = 2; i < dataArr.Length; i++)
        {
            switch (type)
            {
                case ESheetType.EquipmentData:
                    ParseData<CEquipmentDataSO>(dataArr[i]);
                    break;
                case ESheetType.TalentData:
                    ParseData<CTalentDataSO>(dataArr[i]);
                    break;
                case ESheetType.MissionData:
                    ParseData<CMissionDataSO>(dataArr[i]);
                    break;
                case ESheetType.AbilityData:
                    ParseData<CAbilityDataSO>(dataArr[i]);
                    break;
                default:
                    break;
            }
        }

        if (PrintData)
        {
            print(data);
        }
    }

    private void ParseData<T>(string data) where T : ScriptableObject, ICVSData
    {
        T ed = ScriptableObject.CreateInstance<T>();
        ed.ParseData(data);
    }

}
