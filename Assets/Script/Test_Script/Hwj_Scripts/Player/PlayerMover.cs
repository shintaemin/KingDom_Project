using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMover : MonoBehaviour
{
    #region 내부 변수
    private NavMeshAgent _nav;
    private Camera _camera;

    public List<Vector3> wayPoints = new List<Vector3>();
    #endregion

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RecordPath();
        }

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
                Vector3 center = hit.collider.transform.position;

                if (wayPoints.Count == 0 || wayPoints[wayPoints.Count - 1] != center)
                {
                    wayPoints.Add(center);
                }
            }
        }
    }

    private IEnumerator FollowPathRoutine()
    {
        while (wayPoints.Count > 0)
        {
            Vector3 nextTarget = wayPoints[0];
            _nav.SetDestination(nextTarget);

            while (Vector3.Distance(transform.position, nextTarget) > 0.2f)
            {
                yield return null;
            }

            wayPoints.RemoveAt(0);
        }
    }
}