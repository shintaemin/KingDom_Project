using System;
using UnityEngine;


#region TestHPBarTester
/*
▶ 작성자 류연우
*/
#endregion

public class TestHPBarTester : MonoBehaviour
{
    public event Action<float> OnHealthChanged;
    public event Action<Vector3> OnPositionChanged;
    #region 인스펙터

    #endregion

    #region 내부 변수

    #endregion

    void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        float value = Mathf.PingPong(Time.time * 1, 1);
        OnPositionChanged?.Invoke(transform.position);
        OnHealthChanged?.Invoke(value);
    }
}
