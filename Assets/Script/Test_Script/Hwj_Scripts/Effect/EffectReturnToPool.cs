using System.Collections;
using UnityEngine;

/*
    ㆍ EffectReturnToPool

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 
*/

public class EffectReturnToPool : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private EffectManager.EEffectType _type;
    [SerializeField] private float _duration = 1.0f;
    [SerializeField] private float _extraDuration = 0f;
    #endregion

    #region 내부 변수
    private Coroutine _returnRoutine;
    private ParticleSystem[] _particles;
    #endregion

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
        }

        _returnRoutine = StartCoroutine(CoReturnToPool());
    }

    private void OnDisable()
    {
        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
        }
    }

    private IEnumerator CoReturnToPool()
    {
        yield return new WaitForSeconds(_duration);

        if (_particles != null)
        {
            foreach (var p in _particles)
            {
                p.Stop();
            }
        }

        yield return new WaitForSeconds(_extraDuration);

        EffectManager.Instance.DespawnEffect(_type, gameObject);
    }
}