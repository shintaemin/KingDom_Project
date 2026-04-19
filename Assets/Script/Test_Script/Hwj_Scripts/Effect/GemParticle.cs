using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemParticle : MonoBehaviour
{
    #region 내부 변수
    private ParticleSystem _ps;
    private List<ParticleSystem.Particle> _enterGems = new List<ParticleSystem.Particle>();
    private static Collider _playerCollider;
    public static System.Action<int> OnGemCollected;
    private int _perGem = 10;
    #endregion

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(CoCollection(0.65f));
    }

    private void OnDisable()
    {

        _enterGems.Clear();
    }

    private void OnParticleTrigger()
    {
        int gemCount = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, _enterGems);

        if (gemCount > 0)
        {
            for (int i = 0; i < gemCount; i++)
            {
                ParticleSystem.Particle p = _enterGems[i];
                p.remainingLifetime = 0;
                _enterGems[i] = p;
            }

            _ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, _enterGems);

            int rootGem = gemCount * _perGem;

            if (CPlayerDataManager.Instance != null)
            {
                CPlayerDataManager.Instance.Gem = rootGem;
            }

            OnGemCollected?.Invoke(rootGem);
        }
    }

    private IEnumerator CoCollection(float time)
    {
        yield return new WaitForSeconds(time);

        if (_ps != null)
        {
            if (_playerCollider == null)
            {
                GameObject player = GameObject.FindWithTag("Player");

                if (player != null)
                {
                    _playerCollider = player.GetComponent<Collider>();
                }

            }

            if (_playerCollider != null)
            {
                var trigger = _ps.trigger;

                trigger.SetCollider(0, _playerCollider);
            }
        }
    }
}