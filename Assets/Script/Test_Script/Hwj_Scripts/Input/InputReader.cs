using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ InputReader

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 마우스 <-> 터치 조작 방식 전환, 인풋 상태 정보 제공
*/

public class InputReader : MonoBehaviour
{
    #region 인스펙터
    [Header("마우스 <-> 터치 조작 전환")]
    [SerializeField] private bool _useMouseControl = true;
    #endregion

    #region 외부 호출 함수
    public bool GetIsDown()
    {
        if (_useMouseControl)
        {
            return Input.GetMouseButtonDown(0);
        }

        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public bool GetIsHold()
    {
        if (_useMouseControl)
        {
            return Input.GetMouseButton(0);
        }

        return Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary);
    }

    public bool GetIsUp()
    {
        if (_useMouseControl)
        {
            return Input.GetMouseButtonUp(0);
        }

        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
    }

    public Vector3 GetMousePosition()
    {
        if (_useMouseControl)
        {
            return Input.mousePosition;
        }

        return Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Vector3.zero;
    }
    #endregion
}