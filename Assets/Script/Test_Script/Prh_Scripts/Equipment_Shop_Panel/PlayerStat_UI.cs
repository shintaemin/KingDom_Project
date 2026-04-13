using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat_UI : MonoBehaviour
{
    #region 인스펙터
    [Header("텍스트 연결")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI speedText;
    #endregion

    private int _cachedAttack;
    private int _cachedHP;
    private float _cachedMoveSpeed;

    void Start()
    {
        _cachedAttack = -1; // 강제 초기 갱신 유도
        _cachedHP = -1;
        _cachedMoveSpeed = -1f;

        if (CPlayerDataManager.Instance != null)
            UpdateUI();
    }

    /*
    void Update()
    {
        var player = CPlayerDataManager.Instance;
        if (player == null) return;

        if (player.Attack == _cachedAttack &&
            player.HP == _cachedHP &&
            player.MoveSpeed == _cachedMoveSpeed) return; // 변화 없으면 스킵

        _cachedAttack = player.Attack;
        _cachedHP = player.HP;
        _cachedMoveSpeed = player.MoveSpeed;

        UpdateUI();
    }
    */

    public void UpdateUI()
    {
        var player = CPlayerDataManager.Instance;
        if (player == null) return;

        try
        {
            attackText.text = player.Attack.ToString();
            hpText.text = player.HP.ToString();
            speedText.text = (player.MoveSpeed * 100f).ToString("F0") + "%";
        }
        catch
        {
            // 초기화 안된 상태 방어
        }
    }
    

    void OnEnable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged += UpdateUI;
    }

    void OnDisable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= UpdateUI;
    }
}
