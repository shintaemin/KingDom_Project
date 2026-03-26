using UnityEngine;


#region CInGameCamera
/*
▶ 작성자 류연우

적의 유무를 확인하고 플레이어를 추적하는 카메라.

[SerializeField, Range(10f,20f)] private float _height = 15;
 ㄴ 의 값 중 최대값은 맵의 밖이 보이지 않는 값이여야 하고
    최소값은 인게임 상의 지붕보다 높아야 한다.
    맵마다 다른 설정으로 인한 조정이 필요할것으로 보인다.
    유틸리티 등으로 값을 가져오는게 좋을 것 같다.

_leftDownPos 와 _rightUpPos의 높이는 같아야 한다.

============================================================

높이를 역산해내는게 좋을까?

새로운 대상 탐색은 일정 시간마다 한다.
추적 대상 중 없어지는 객체가 생겨나면?
    ㄴ 일단 멈춘다
    ㄴ 즉시 새로운 추적을 시작한다.
*/
#endregion

public class CInGameCamera : MonoBehaviour
{
    public enum ECameraMoveType
    {
        Fix,
        MoveTowards,    // 원작 게임은 이걸로 추청. 그래서 ㅈㄴ 답답함.
        Lerp,
        SmoothDamp
    }

    #region 인스펙터
    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _enemy;

    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _leftDownPos;
    [SerializeField] private Transform _rightUpPos;

    [SerializeField, Range(10f, 20f)] private float _height = 15;

    [SerializeField] private float _fov = 60;

    [SerializeField] private float _clampGap = 1;

    [SerializeField] private ECameraMoveType _cameraType = ECameraMoveType.MoveTowards;

    [Header("MoveTowards")]
    [SerializeField] private float _cameraSpeed = 10;

    [Header("기즈모")]
    [SerializeField] private bool _drawfloor = true;
    [SerializeField] private bool _drawCamerafloor = true;
    [SerializeField] private bool _drawCameraFrustum = true;

    [Header("디버그용 필드")]
    [SerializeField] private bool _CheckEnemy = true;
    #endregion

    #region 내부 변수
    private float _cameraMinX;
    private float _cameraMaxX;
    private float _cameraMinZ;
    private float _cameraMaxZ;

    private float _hX;
    private float _hZ;
    #endregion

    void Awake()
    {
        if (
        ! _camera.NullCheck("_camera") ||
        ! _player.NullCheck("_player") ||
        ! _leftDownPos.NullCheck("_leftDownPos") ||
        ! _rightUpPos.NullCheck("_rightUpPos")
        ) return;
    }

    void Start()
    {
        InitCameraSetting();

        // 카메라 해상도와 높이값 을 통한 clamp용 값 계산
        float halfRadian = (_fov * 0.5f) * Mathf.Deg2Rad;
        _hZ = Mathf.Tan(halfRadian) * (_height - _leftDownPos.position.y);

        _hX = _hZ * UnityEngine.Device.Screen.width / (float)UnityEngine.Device.Screen.height;

        //_hX -= _clampGap;
        //_hZ -= _clampGap;

        _cameraMinX = _leftDownPos.position.x + _hX;
        _cameraMinZ = _leftDownPos.position.z + _hZ;
        _cameraMaxX = _rightUpPos.position.x - _hX;
        _cameraMaxZ = _rightUpPos.position.z - _hZ;
    }

    void Update()
    {
        UpdateCameraTransform();
    }

    private void InitCameraSetting()
    {
        _camera.fieldOfView = _fov;
        // 추후 맵 아래에 무언가 추가된다면 이 상수도 필드로 만든다.
        // 깊이에 따라서 그림자가 달라지는데?
        _camera.farClipPlane = _height + 5f;

        UpdateCameraTransform();

    }

    private void UpdateCameraTransform()
    {
        Vector3 pos = _player.position;

        // x,z값 계산
        // 적의 유무 파악


        if (GetNearestEnemy(out Transform nearestEnemy))
        {
            pos = (pos + nearestEnemy.position) * 0.5f;
        }

        // 높이는 고정
        pos.y = _height;

        // 플레이어가 보이기는 해야한다.
        // 지금 pos를 플레이어 위치를 기준으로 clamp 한다.
        pos.x = Mathf.Clamp(pos.x, _player.position.x - _hX + _clampGap, _player.position.x + _hX - _clampGap);
        pos.z = Mathf.Clamp(pos.z, _player.position.z - _hZ + _clampGap, _player.position.z + _hZ - _clampGap);

        // 위치 clamp
        pos.x = Mathf.Clamp(pos.x, _cameraMinX, _cameraMaxX);
        pos.z = Mathf.Clamp(pos.z, _cameraMinZ, _cameraMaxZ);

        switch (_cameraType)
        {
            case ECameraMoveType.MoveTowards:
                _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, pos, _cameraSpeed * Time.deltaTime);
                break;
            default:
                _camera.transform.position = pos;
                break;
        }
    }

    private bool GetNearestEnemy( out Transform nearestEnemy)
    {
        if (_CheckEnemy && _enemy != null && _enemy.Length != 0)
        {
            // 가장 가까운 적을 가져온다.
            nearestEnemy = _enemy[0];
            float nearestDist = Vector3.SqrMagnitude(_player.transform.position - nearestEnemy.position);

            for (int i = 1; i < _enemy.Length; i++)
            {
                float dist = Vector3.SqrMagnitude(_player.transform.position - _enemy[i].position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestEnemy = _enemy[i];
                }
            }
            return true;
        }
        nearestEnemy = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_camera == null) return;

        // 평면
        if (_drawfloor)
        {
            Gizmos.color = Color.blue;

            Vector3 center = (_leftDownPos.position + _rightUpPos.position) * 0.5f;
            //center.y -= _leftDownPos.position.y;
            Vector3 size = new Vector3(
                Mathf.Abs(_rightUpPos.position.x - _leftDownPos.position.x),
                Mathf.Abs(_rightUpPos.position.y - _leftDownPos.position.y),
                Mathf.Abs(_rightUpPos.position.z - _leftDownPos.position.z)
            );
            Gizmos.DrawWireCube(center, size);
        }


        // 카메라 위치 
        if (_drawCamerafloor)
        {
            Gizmos.color = Color.blue;

            Vector3 center = new Vector3((_cameraMinX + _cameraMaxX) * 0.5f, _height, (_cameraMinZ + _cameraMaxZ) * 0.5f);
            Vector3 size = new Vector3(
                Mathf.Abs(_cameraMaxX - _cameraMinX),
                0f,
                Mathf.Abs(_cameraMaxZ - _cameraMinZ)
            );

            Gizmos.DrawWireCube(center, size);
        }

        // 절두체
        if (_drawCameraFrustum)
        {
            Gizmos.color = Color.red;

            Gizmos.matrix = Matrix4x4.TRS(_camera.transform.position, _camera.transform.rotation, Vector3.one);

            Gizmos.DrawFrustum(Vector3.zero, _camera.fieldOfView, _camera.farClipPlane, _camera.nearClipPlane, _camera.aspect);
            Gizmos.DrawFrustum(Vector3.zero, _camera.fieldOfView, _height - _leftDownPos.position.y, _camera.nearClipPlane, _camera.aspect);
        }
    }
}
