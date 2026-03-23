using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class EnemyMover : MonoBehaviour
{
    #region 인스펙터

    #endregion

    #region 내부 변수
    private EnemyState _state;
    #endregion

    void Start()
    {
        _state = GetComponent<EnemyState>();

        if (_state == null)
        {
            Debug.LogError("");
            return;
        }
    }

    private void OnEnable()
    {
        if (_state != null)
        {
            _state.OnStateChanged += StateChanged;
        }
    }

    private void OnDisable()
    {
        if (_state != null)
        {
            _state.OnStateChanged -= StateChanged;
        }
    }

    void Update()
    {

    }

    private void StateChanged(EnemyState.EState state)
    {
        switch (state)
        {
            case EnemyState.EState.Patrol:

                break;

            case EnemyState.EState.Detect:

                break;

            case EnemyState.EState.Chase:

                break;

            case EnemyState.EState.ChaseFail:

                break;
        }
    }
}
