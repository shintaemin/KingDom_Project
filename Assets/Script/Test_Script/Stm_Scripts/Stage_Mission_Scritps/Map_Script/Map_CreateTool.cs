using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

#region 맵 제작 툴
/*
 ▶ 할일
  - 프리펩을 넣고 추가할 컴포넌트를 지정하고 위치, 방향, 갯수 를 지정해 프리펩을 생성하는 스크립트
*/
#endregion


[System.Serializable]
public class SetDir
{
    public enum EDirType
    {
        None,
        Left,
        Right,
        Forward,
        Back,
    }

    public EDirType dir;

    public Vector3 GetDir()
    {
        switch(dir)
        {
            case EDirType.Left:
                return Vector3.left;
            case EDirType.Right:
                return Vector3.right;
            case EDirType.Forward:
                return Vector3.forward;
            case EDirType.Back:
                return Vector3.back;
        }
        return Vector3.zero;
    }
}

[ExecuteInEditMode]
public class Map_CreateTool : MonoBehaviour
{
    #region 인스펙터
    [Header("프리펩")]
    [SerializeField] private GameObject _prefab;

    [Header("부모설정")]
    [SerializeField] private Transform _root;

    [Header("외형설정")]
    [SerializeField] private Material _mat;

    [Header("레이어 설정")]
    [SerializeField] private int _layerIndex = 0;

    [Header("갯수 설정")]
    [SerializeField] private int _count = 1;

    [Header("시작 위치")]
    [SerializeField] private Vector3 _startPos;

    [Header("방향")]
    [SerializeField] private SetDir _dir;

    [Header("스케일")]
    [SerializeField] private Vector3 _scail = Vector3.one;

    [Header("옵션")]
    [SerializeField] private bool _addBoxCol;
    [SerializeField] private bool _addNavMesh;

    [Header("위 옵션으로 만들기")]
    [SerializeField] private bool _create;
    #endregion

    private void Update()
    {
        if (_count == 0 )
        {
            return;
        }

        if (_create)
        {
            _create = false;
            SetCreateObject();
            return;
        }

    }

    private void SetCreateObject()
    {
        if (_prefab == null)
        {
            return;
        }
        if (_layerIndex == default)
        {
            Debug.LogWarning($"[] : 레이어 지정 안됨 생성 불가");
            return;
        }
        if (_count == 0)
        {
            Debug.LogWarning($"[] : 카운트 지정안됨 생성 불가");
            return;
        }

        Transform root = new GameObject("OBJ_Root").transform;
        root.position = _startPos;

        if (_root != null)
        {
            root.transform.parent = _root;
        }

        Vector3 dir = _dir.GetDir();

        for (int i = 0; i < _count; i++)
        {
            GameObject go = Instantiate(_prefab);
            go.name = $"{_prefab.name}_{i + 1}";
            go.transform.parent = root;
            go.layer = _layerIndex;
            go.transform.position = _startPos + dir * i;
            go.transform.localScale = _scail;

            if (_mat != null)
            {
                Renderer rend = go.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    rend.material = _mat;
                }
            }

            if (_addBoxCol)
            {
                go.AddComponent<BoxCollider>();
            }
            if (_addNavMesh)
            {
                go.AddComponent<NavMeshModifier>();
            }
        }
    }
}
