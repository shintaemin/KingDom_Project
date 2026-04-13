using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EPlayer_Stat_UI : MonoBehaviour
{
    #region 인스펙터
    [Header("텍스트 연결")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI speedText;
    #endregion

    void Start()
    {
        UpdateUI(); // 시작 시 1회 갱신
    }

    void Update()
    {
        UpdateUI(); // 항상 최신값 유지
    }

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
}
