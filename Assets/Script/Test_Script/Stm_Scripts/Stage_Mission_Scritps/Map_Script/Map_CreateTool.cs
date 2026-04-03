using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 맵 제작 툴
/*
 ▶ 할일
  - 프리펩을 넣고 추가할 컴포넌트를 지정하고 위치, 방향, 갯수 를 지정해 프리펩을 생성하는 스크립트
*/
#endregion

[ExecuteInEditMode]
public class Map_CreateTool : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameObject _prefab;
    [SerializeField] private string _setLayerName;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private Vector3 _scail;
    [SerializeField] private int _count;
    [SerializeField] private bool _addBoxCol;
    [SerializeField] private bool _addNavMesh;
    #endregion

    #region

    #endregion

    private void OnValidate()
    {
        SetCreateObject();
    }

    private void SetCreateObject()
    {
        if (_prefab == null)
        {
            return;
        }
        if (_setLayerName == null)
        {
            Debug.LogWarning($"[] : 레이어 지정 안됨 생성 불가");
            return;
        }
        if (_count == 0)
        {
            Debug.LogWarning($"[] : 카운트 지정안됨 생성 불가");
            return;
        }

    }
}
