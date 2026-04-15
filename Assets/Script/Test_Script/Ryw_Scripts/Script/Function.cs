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
    public static bool IsNull<T>(this T obj, string varName, Action body = null) where T : class
    {
        if (obj is UnityEngine.Object unityObj)
        {
            if (unityObj == null)
            {
                Debug.LogWarning($"[Unity Object] {varName} is null or destroyed.");
                body?.Invoke();
                return true;
            }
        }
        else
        {
            if (obj == null)
            {
                Debug.LogWarning($"{varName} == null");
                body?.Invoke();
                return true;
            }
        }

        return false;
    }
    public static bool IsNull<T>(this ICollection<T> objArr, string varName, Action body = null) where T : class
    {
        if (objArr == null || objArr.Count == 0)
        {
            Debug.LogWarning($"{varName} == null || obj.Length == 0");
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
                else if (type == typeof(Sprite))
                {
                    path = CGSSLoader.Sprite_PATH + "/" + data.Trim().Replace("\r", "");
                    Sprite _sprite = Resources.Load<Sprite>(path);
                    if (_sprite == null)
                        Debug.Log($"Sprite == null. {path}");
                    result = _sprite;
                }
                else if (type == typeof(Sprite[]))
                {
                    strings = data.Split(';');

                    Sprite[] _spriteArr = new Sprite[strings.Length];

                    for (int i = 0; i < strings.Length; i++)
                    {
                        path = CGSSLoader.Sprite_PATH + "/" + strings[i].Trim().Replace("\r", "");

                        _spriteArr[i] = Resources.Load<Sprite>(path);
                        if (_spriteArr[i] == null)
                            Debug.Log($"_texture2DArr[{i}] == null. {path}");
                    }

                    result = _spriteArr;
                }
                else if (type == typeof(Mesh))
                {
                    path = CGSSLoader.Mesh_PATH + "/" + data.Trim().Replace("\r", "");
                    Mesh _mesh = Resources.Load<Mesh>(path);
                    if (_mesh == null)
                        Debug.Log($"Mesh == null. {path}");
                    result = _mesh;
                }
                else
                    result = null;
                break;
        }

        return (T)result;
    }

    /// <summary>
    /// MakeSaveData 에서 사용
    /// </summary>
    /// <typeparam name="T"> 딕셔너리 키의 타입</typeparam>
    /// <typeparam name="T2"> 딕셔너리 값의 타입</typeparam>
    /// <param name="obj"> 딕셔너리</param>
    /// <param name="keys"> 키의 배열</param>
    /// <param name="values"> 값의 배열</param>
    public static void DicToArray<T, T2>(this Dictionary<T, T2> obj, T[] keys, T2[]values)
    {
        obj.IsNull("obj");
        keys.IsNull("keys");
        values.IsNull("values");

        // 순서 정렬은 필요 없고 같은 인덱스의 값이 연결만 되면 된다.
        int index = 0;
        foreach (var (key, value) in obj)
        {
            keys[index] = key;
            values[index] = value;
            ++index;
        }
    }

    /// <summary>
    /// LoadSaveData 에서 사용
    /// </summary>
    /// <typeparam name="T">딕셔너리 키의 타입</typeparam>
    /// <typeparam name="T2">딕셔너리 값의 타입</typeparam>
    /// <param name="obj">딕셔너리</param>
    /// <param name="keys">키의 배열</param>
    /// <param name="values">값의 배열</param>
    /// <param name="clearFlag">딕셔너리 clear 여부</param>
    public static void ArrayToDic<T, T2>(this Dictionary<T, T2> obj, T[] keys, T2[] values, bool clearFlag = false)
    {
        obj.IsNull("obj");
        keys.IsNull("keys");
        values.IsNull("values");

        if(clearFlag)
            obj.Clear();

        for (int i = 0; i < keys.Length; i++)
        {
            T id = keys[i];
            T2 value = values[i];
            obj[id] = value;
        }
    }
}
