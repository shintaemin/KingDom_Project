using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ㆍ AfterImageEffect

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class AfterImageEffect : MonoBehaviour
{
    #region 인스펙터
    [Header("잔상 설정")]
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private float _interval = 0.2f;
    [SerializeField] private float _lifeTime = 0.5f;
    [SerializeField] private float _startAlpha = 0.5f;
    [SerializeField] private float _endAlpha = 0f;
    [SerializeField] private Color _color = Color.white;
    #endregion

    #region 내부 변수
    private PlayerMover _playerMover;
    private float _lastTime;
    #endregion

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    private void OnEnable()
    {
        if (_playerMover != null)
        {
            _playerMover.OnBackMove += BackMove;
        }
    }

    private void OnDisable()
    {
        if (_playerMover != null)
        {
            _playerMover.OnBackMove -= BackMove;
        }
    }

    private void BackMove()
    {
        if (Time.time < _lastTime + _interval)
        {
            return;
        }

        _lastTime = Time.time;

        Mesh mesh = new Mesh();
        _meshRenderer.BakeMesh(mesh);

        GameObject effect = Instantiate(_gameObject, _meshRenderer.transform.position, _meshRenderer.transform.rotation);
        effect.GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CoFade(effect, mesh));
    }

    private IEnumerator CoFade(GameObject gameObject, Mesh mesh)
    {
        MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();

        Material mat = rend.material;

        float timer = 0f;

        while (timer < _lifeTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(_startAlpha, _endAlpha, timer / _lifeTime);

            Color c = new Color();

            c = _color;
            c.a = alpha;
            mat.color = c;

            yield return null;
        }

        Destroy(gameObject);
        Destroy(mesh);
    }
}
