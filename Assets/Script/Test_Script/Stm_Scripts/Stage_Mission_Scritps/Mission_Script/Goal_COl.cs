using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 골 콜라이더
/*
 ▶ 할일
  - 미션매니저를 캐싱하고 지정된 태그 충돌시 GoalEvent 호출
*/
#endregion


public class Goal_COl : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private MissionManager _msManager;
    [SerializeField] private string _colTag = "Player";
    [SerializeField] private bool _isCol = false;
    #endregion

    #region

    #endregion

    private void Start()
    {
        if (_msManager == null)
        {
            _msManager = FindFirstObjectByType<MissionManager>();   
        }

        _isCol = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCol || _msManager == null || !other.CompareTag(_colTag))
        {
            return;
        }

        Debug.Log($"[Goal_COl] : 골인! 충돌 완료");
        _isCol = true;
        _msManager.GoalEvent();
    }
}
