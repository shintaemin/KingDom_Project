using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot_Data : MonoBehaviour
{
    public Image icon;
    public TMP_Text levelText;
    public TMP_Text valueText;
    public TMP_Text priceText;
    public Animator effectAnimator;

    private int level = 1;
    private int value;
    private CAbilityDataSO data;

    public void Init(CAbilityDataSO so)
    {
        data = so;
        level = 1;
        value = 0;

        UpdateUI();
    }

    public void OnClickUpgrade()
    {
        if (level >= data.Capacity)
            return;

        level++;
        value += data.Val;

        if (effectAnimator != null)
        {
            effectAnimator.Play("UpgradeEffect", -1, 0f);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        // 레벨
        levelText.text = "LV. " + level;

        // 값
        valueText.text = "+" + value;

        // 가격
        if (data.PriceArr != null && level - 1 < data.PriceArr.Length)
            priceText.text = data.PriceArr[level - 1].ToString();
        else
            priceText.text = "-";

        // 아이콘
        if (data.IconArr != null && data.IconArr.Length > 0)
        {
            int iconIndex = (level - 1) / 10;
            iconIndex = Mathf.Clamp(iconIndex, 0, data.IconArr.Length - 1);

            icon.sprite = data.IconArr[iconIndex];
        }
    }
}
