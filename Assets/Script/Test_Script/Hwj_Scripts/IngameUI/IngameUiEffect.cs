using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameUiEffect : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어 태그")]
    [SerializeField] private string _playerTag = "Player";

    [Header("텍스트 공용 y보정값")]
    [SerializeField] private float _yOffset = 4f;

    [Header("대미지 텍스트 설정")]
    [SerializeField] private float _dmgDuration = 1f;
    [SerializeField] private float _randX = 0.5f;
    [SerializeField] private float _randZ = 0.5f;

    [Header("잼 텍스트 설정")]
    [SerializeField] private float _gemDuration = 1f;
    #endregion

    #region 내부 변수
    private Transform _playerTr;
    private int _totalGem = 0;
    #endregion

    private void OnEnable()
    {
        GemParticle.OnGemCollected += GemCollected;
        PlayerCombat.OnPlayerHitTarget += PlayerHitTarget;

        StartCoroutine(CoBindPlayer());
    }

    private void OnDisable()
    {
        GemParticle.OnGemCollected -= GemCollected;
        PlayerCombat.OnPlayerHitTarget -= PlayerHitTarget;
    }

    private IEnumerator CoBindPlayer()
    {
        while (_playerTr == null)
        {
            var player = GameObject.FindWithTag(_playerTag);

            if (player != null)
            {
                _playerTr = player.transform;
            }

            else
            {
                yield return null;
            }
        }
    }

    private void GemCollected(int amount)
    {
        GameObject go = ProjectileManager.Instance.SpawnProjectile(ProjectileManager.EProjectileType.GemText);

        if (go != null)
        {
            go.transform.position = _playerTr.position + Vector3.up * _yOffset;

            var gemText = go.GetComponentInChildren<TextMeshProUGUI>();

            _totalGem += amount;

            gemText.text = $"+{_totalGem}";
        }
    }

    private void PlayerHitTarget(Vector3 pos, float dmg, bool isBack)
    {
        GameObject go = ProjectileManager.Instance.SpawnProjectile(ProjectileManager.EProjectileType.DamageText);

        if (go != null)
        {
            Vector3 startPos = pos + Vector3.up * 4f;
            float randX = Random.Range(-_randX, _randX);
            float randZ = Random.Range(-_randZ, _randZ);

            go.transform.position = new Vector3
                (
                startPos.x + randX,
                startPos.y,
                startPos.z + randZ
                );

            var dmgtext = go.GetComponentInChildren<TextMeshProUGUI>();

            dmgtext.text = dmg.ToString();

            if (isBack)
            {
                dmgtext.color = Color.red;
            }

            else
            {
                dmgtext.color = Color.yellow;
            }
        }

        StartCoroutine(CoDamageTextRoutine(go));
    }

    private IEnumerator CoDamageTextRoutine(GameObject go)
    {
        var dmgtext = go.GetComponentInChildren<TextMeshProUGUI>();
        Color c = dmgtext.color;
        Vector3 pos = go.transform.position;

        float timer = 0f;

        while (timer < _dmgDuration)
        {
            timer += Time.deltaTime;

            go.transform.position = pos + Vector3.forward * timer;

            Color nc = c;
            nc.a = Mathf.Lerp(1f, 0f, timer);
            dmgtext.color = nc;

            yield return null;
        }

        ProjectileManager.Instance.DespawnProjectile(ProjectileManager.EProjectileType.DamageText, go);
    }
}