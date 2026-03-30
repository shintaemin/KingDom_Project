using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    ㆍ PlayerMover

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class PlayerMover : MonoBehaviour
{
    #region 인스펙터
    [Header("거리 설정")]
    [SerializeField] private float _arrivedEnemyDistance = 0.5f;
    [SerializeField] private float _pathDistanceOffset = 0.1f;

    [Header("기본 이동속도 설정")]
    [SerializeField] private float _baseSpeed = 5f;
    #endregion

    #region 내부 변수
    private NavMeshAgent _nav;
    private InputState _inputState;
    private PlayerState _playerState;
    private Coroutine _moveRoutine;
    #endregion

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _inputState = GetComponent<InputState>();
        _playerState = GetComponent<PlayerState>();

        if (_nav == null || _inputState == null || _playerState == null)
        {
            Debug.LogError("PlayerMover _nav _inputState _playerState 참조 실패");
            return;
        }
    }

    private void OnEnable()
    {
        if (_inputState != null)
        {
            _inputState.OnStateChanged += StateChanged;
        }
    }

    private void OnDisable()
    {
        if (_inputState != null)
        {
            _inputState.OnStateChanged -= StateChanged;
        }
    }

    void Update()
    {
        if (_inputState.GetState() == InputState.EState.Drawing)
        {
            
        }
    }

    private void StateChanged(InputState.EState state)
    {
        switch (state)
        {
            case InputState.EState.Start:

                break;

            case InputState.EState.End:

                break;
        }
    }

    #region 외부 호출 함수
    public void SetMoveSpeed(float speed)
    {
        if (_nav == null)
        {
            _nav = GetComponent<NavMeshAgent>();
        }

        _nav.speed = _baseSpeed * (speed / 100f);

        _nav.acceleration = _baseSpeed * 10f * (speed / 100f);
    }
    #endregion
}