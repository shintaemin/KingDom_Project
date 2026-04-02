using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region 인게임 센터 UI 지우기
/*
 ▶ 할일
  - 일정시간이 지나면 해당 UI 비활성화 후 Null 처리
  - 시간이 지나면 스크립트 비활성화까지
*/
#endregion


public class InGame_Center_UI_Hide : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _centerUI;
    [SerializeField] private float _hideTime = 5.0f;
    #endregion

    #region 내부 변수
    private float _nextHideTime = 0;
    #endregion

    private void Start()
    {
        _nextHideTime = Time.time + _hideTime;
    }

    private void Update()
    {
        if (_centerUI == null)
        {
            return;
        }

        if (Time.time >= _nextHideTime)
        {
            _centerUI.SetActive(false);
            _centerUI = null;
            enabled = false;
        }
    }
}
