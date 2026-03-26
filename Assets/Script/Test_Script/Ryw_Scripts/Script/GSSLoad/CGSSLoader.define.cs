

#region CGSSLoader.define
/*
▶ 작성자 류연우

파싱과 불러오기에 사용할 각종 파일 경로들을 Define 하기 위한 파일.
*/
#endregion

using UnityEngine;

public partial class CGSSLoader : MonoBehaviour
{
    #region 스프레드시트
    static readonly string URL = "https://docs.google.com/spreadsheets/d/1wx7tsBCYFjxJkCGeNdoklLhEJamttKUGMLcJNghr1wc";
    static readonly string EXTRA_URL = "/export?format=";
    static readonly string LOAD_TYPE = "csv";
    // sheet 페이지별 gid의 배열.
    static readonly string[] SHEET_NUMBER = new string[] { "", "&gid=214534590#gid=214534590", "&gid=299399325#gid=299399325", "&gid=2096374625#gid=2096374625" };
    #endregion

    #region 파일 경로
    public static readonly string SO_PATH = "Assets/Script/Test_Script/Ryw_Scripts/SO";
    public static readonly string Texture2D_PATH = "Texture2D";
    #endregion
}
