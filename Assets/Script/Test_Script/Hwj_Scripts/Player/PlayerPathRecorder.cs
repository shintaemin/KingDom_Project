using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ PlayerPathRecorder

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class PlayerPathRecorder : MonoBehaviour
{
    #region 인스펙터
    [Header("레이어 설정")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _terrainLayer;
    #endregion

    #region 내부 변수
    private List<Vector3> _wayPoints = new List<Vector3>();
    private InputReader _inputReader;
    private Camera _camera;
    private Transform _enemyTr;
    #endregion

    private void Awake()
    {
        _camera = Camera.main;
        _inputReader = GetComponent<InputReader>();

        if (_inputReader == null)
        {
            Debug.LogError("PlayerPathRecorder _inputReader 참조 실패");
            return;
        }
    }

    #region 외부 호출 함수
    public void RecordPath()
    {
        Ray ray = _camera.ScreenPointToRay(_inputReader.GetInputPosition());

        if (Physics.Raycast(ray, out RaycastHit hitEnemy, Mathf.Infinity, _enemyLayer))
        {
            _enemyTr = hitEnemy.transform;
            _wayPoints.Clear();
        }

        else if (Physics.Raycast(ray, out RaycastHit hitTerrain, Mathf.Infinity, _terrainLayer))
        {
            Vector3 center = hitTerrain.transform.position;

            center.y = transform.position.y;

            // 이미 웨이포인트에 존재하는 위치일 경우 해당 위치 웨이포인트 제거
            if (_wayPoints.Contains(center))
            {
                int index = _wayPoints.IndexOf(center);

                if (index < _wayPoints.Count - 1)
                {
                    _wayPoints.RemoveRange(index + 1, _wayPoints.Count - (index + 1));
                }
            }

            // 새로운 웨이포인트일 경우 추가
            else
            {
                _wayPoints.Add(center);
                _enemyTr = null;
            }
        }
    }

    public void ResetPath()
    {
        _wayPoints.Clear();
        _enemyTr = null;
    }

    public List<Vector3> GetPath()
    {
        return _wayPoints;
    }

    public Transform GetEnemy()
    {
        return _enemyTr;
    }
    #endregion
}