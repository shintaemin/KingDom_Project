using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 천장 하이딩
/*
 ▶ 할일
  - 플레이어 충돌이 감지되면 천장 오브젝트 루트 비활성화
*/
#endregion


public class Ceiling_Hiding : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _ceilingRoot;
    [SerializeField] private string _colTargetTag = "Player";
    [SerializeField] private bool _view;
    #endregion

    #region

    #endregion

    private void Awake()
    {
        if (_ceilingRoot == null)
        {
            Debug.LogWarning($"[Ceiling_Hiding] : 천장 오브젝트 루트가 없음");
        }

        CeilingView(true);
    }

    private void CeilingView(bool view)
    {
        if (_ceilingRoot == null)
        {
            return;
        }

        _view = view;
        _ceilingRoot.SetActive(_view);
        Debug.Log($"[CeilingView] : 천장 {_view}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_colTargetTag))
        {
            return;
        }
        
        CeilingView(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_colTargetTag))
        {
            return;
        }

        CeilingView(true);
    }
}
