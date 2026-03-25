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
    public static T ParseData<T>(this T obj, string data)
    {
        object result = obj switch
        {
            int _ => int.Parse(data),
            float _ => float.Parse(data),
            double _ => double.Parse(data),
            string _ => data,
            Enum e => Enum.Parse(e.GetType(), data, true),
            _ => null
        };

        return (T)result;
    }
}



