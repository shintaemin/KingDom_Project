using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMover : MonoBehaviour
{
    #region 내부 변수
    private NavMeshAgent _nav;
    private Camera _camera;

    public List<Vector3> pathPoints = new List<Vector3>();
    #endregion

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    void Update()
    {
        // 1. 드래그 중: 경로 수집
        if (Input.GetMouseButton(0))
        {
            RecordPath();
        }

        // 2. 마우스를 뗀 순간: 이동 시작 명령 (필요 시 플래그 세팅)
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(FollowPathRoutine());
        }
    }

    private void RecordPath()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                Vector3 tileCenter = hit.collider.transform.position;

                // 리스트의 마지막 좌표와 지금 찍은 타일 좌표가 다를 때만 추가
                if (pathPoints.Count == 0 || pathPoints[pathPoints.Count - 1] != tileCenter)
                {
                    pathPoints.Add(tileCenter);
                    // 여기서 LineRenderer를 갱신하면 실시간으로 선이 그려집니다!
                }
            }
        }
    }

    private IEnumerator FollowPathRoutine()
    {
        // 리스트에 쌓인 좌표가 없을 때까지 반복
        while (pathPoints.Count > 0)
        {
            Vector3 nextTarget = pathPoints[0]; // 맨 앞의 좌표를 가져옴
            _nav.SetDestination(nextTarget);

            // 해당 지점에 거의 도착했는지 체크 (거리가 0.1m 이하일 때)
            while (Vector3.Distance(transform.position, nextTarget) > 0.2f)
            {
                yield return null; // 도착할 때까지 대기
            }

            pathPoints.RemoveAt(0); // 도착했으니 리스트에서 제거
        }
    }
}