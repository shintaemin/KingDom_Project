using System;
using System.Collections.Generic;
using UnityEngine;


#region Function
/*


대문자 변환 ctrl + shift + U
소문자 변환 ctrl + U

*/
#endregion


public static partial class Function
{
    public static bool NullCheck<T>(this T obj, string varName, Action body = null) where T : class
    {
        if (obj is UnityEngine.Object unityObj)
        {
            if (unityObj == null)
            {
                Debug.Log($"[Unity Object] {varName} is null or destroyed.");
                body?.Invoke();
                return true;
            }
        }
        else
        {
            if (obj == null)
            {
                Debug.Log($"{varName} == null");
                body?.Invoke();
                return true;
            }
        }

        return false;
    }
    public static bool NullCheck<T>(this ICollection<T> objArr, string varName, Action body = null) where T : class
    {
        if (objArr == null || objArr.Count == 0)
        {
            Debug.Log($"{varName} == null || obj.Length == 0");
            body?.Invoke();
            return true;
        }
        return false;
    }

    // 확인만 하고 값은 사용하지 않을 때 _를 붙인다. 아니라면 enum처럼 변수 이름 을 지어 붙인다.
    // 박싱, 언박싱을 하므로 속도가 느리다.
    // 더 효율적인 방법을 찾아보자..
    public static T ParseData<T>(this T obj, string data)
    {
        object result;

        string[] strings;

        string path;

        switch (obj)
        {
            case int _:
                result = int.Parse(data);
                break;

            case int[] _:
                strings = data.Split(';');
                int[] iArr = new int[strings.Length];
                for (int i = 0; i < iArr.Length; i++)
                {
                    iArr[i] = int.Parse(strings[i]);
                }
                result = iArr;
                break;

            case float _:
                result = float.Parse(data);
                break;

            case float[] _:
                strings = data.Split(';');
                float[] fArr = new float[strings.Length];
                for (int i = 0; i < fArr.Length; i++)
                {
                    fArr[i] = float.Parse(strings[i]);
                }
                result = fArr;
                break;

            case double _:
                result = double.Parse(data);
                break;

            case double[] _:
                strings = data.Split(';');
                double[] dArr = new double[strings.Length];
                for (int i = 0; i < dArr.Length; i++)
                {
                    dArr[i] = double.Parse(strings[i]);
                }
                result = dArr;
                break;

            case string _:
                result = data;
                break;

            case string[] _:
                result = data.Split(';');
                break;

            case Enum e:
                result = Enum.Parse(e.GetType(), data, true);
                break;

            default:
                // 이 분기로 들어오는 대부분은 파일 이름의 배열일것.
                Type type = typeof(T);

                if (type == typeof(Texture2D))
                {
                    path = CGSSLoader.Texture2D_PATH + "/" + data.Trim().Replace("\r", "");
                    Texture2D _texture2D = Resources.Load<Texture2D>(path);
                    if (_texture2D == null)
                        Debug.Log($"_texture2D == null. {path}");
                    result = _texture2D;
                }
                else if (type == typeof(Texture2D[]))
                {
                    strings = data.Split(';');

                    Texture2D[] _texture2DArr = new Texture2D[strings.Length];

                    for (int i = 0; i < strings.Length; i++)
                    {
                        path = CGSSLoader.Texture2D_PATH + "/" + strings[i].Trim().Replace("\r", "");

                        _texture2DArr[i] = Resources.Load<Texture2D>(path);
                        if (_texture2DArr[i] == null)
                            Debug.Log($"_texture2DArr[{i}] == null. {path}");
                    }

                    result = _texture2DArr;
                }
                else
                    result = null;
                break;
        }

        return (T)result;
    }
}
