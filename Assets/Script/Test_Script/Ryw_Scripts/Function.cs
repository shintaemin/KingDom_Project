using System;
using UnityEngine;


#region Function
/*

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
}



