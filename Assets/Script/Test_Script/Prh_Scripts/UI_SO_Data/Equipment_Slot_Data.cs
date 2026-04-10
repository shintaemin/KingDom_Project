using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region 장비상점 슬롯 UI 데이터 적용
/*
 ▶ 할일
  - 장비 데이터를 받아 UI에 표시
  - 이미지 / 스탯 텍스트 적용
  - 선택 여부(체크 표시) 처리

 ▶ 흐름
  1. SetData(id) 호출 → SO 데이터 가져오기
  2. CSOManager에서 ID로 데이터 가져오기
  3. 이미지 / 텍스트 UI 반영
  4. 선택 상태는 기본 false

  - 박라희
*/
#endregion

public class Equipment_Slot_Data : MonoBehaviour
{
    #region 인스펙터
    [Header("아이콘")]
    [SerializeField] private Image lockIcon;
    [SerializeField] private Image openIcon;
    [SerializeField] private Image checkIcon;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI openStatText;
    [SerializeField] private TextMeshProUGUI checkStatText;

    [Header("ID")]
    [SerializeField] private int id;
    #endregion

    private CEquipmentDataSO data;

    private void Start()
    {
        SetData(id);
    }

    public void SetData(int id)
    {
        data = CSOManager.Instance.SuchData<CEquipmentDataSO>(
            CDataArraySO.EDataType.EquipmentData,
            id
        );

        if (data == null)
        {
            Debug.LogError("데이터 없음: " + id);
            return;
        }

        
        if (openIcon != null)
            openIcon.sprite = data.Image;

        if (checkIcon != null)
            checkIcon.sprite = data.Image;

        if (lockIcon != null)
        {
            lockIcon.sprite = data.Image;
            lockIcon.color = Color.black; // 락만 흑백
        }

        SetStatText();
    }

    private void SetStatText()
    {
        if (data == null) return;

        string text = "";

        if (data.AdditionalAttackRatio > 0)
            text = "+" + data.AdditionalAttackRatio + "%";
        else if (data.AdditionalHealthRatio > 0)
            text = "+" + data.AdditionalHealthRatio + "%";
        else if (data.AdditionalSpeedRatio > 0)
            text = "+" + data.AdditionalSpeedRatio + "%";

        if (string.IsNullOrEmpty(text))
            return;

        if (openStatText != null)
        {
            openStatText.text = text;
            openStatText.fontSize = 36;
            openStatText.fontStyle = TMPro.FontStyles.Bold;
            openStatText.color = Color.white;
        }

        if (checkStatText != null)
        {
            checkStatText.text = text;
            checkStatText.fontSize = 36;
            checkStatText.fontStyle = TMPro.FontStyles.Bold;
        }
    }

    public CEquipmentDataSO GetData()
    {
        return data;
    }

    /*
    public void SetSelected(bool value)
    {
        // 다른 스크립트에서 처리
    }
    */
}
