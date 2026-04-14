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
    [SerializeField] private Image _lockIcon;
    [SerializeField] private Image _openIcon;
    [SerializeField] private Image _checkIcon;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI _openStatText;
    [SerializeField] private TextMeshProUGUI _checkStatText;

    [Header("ID")]
    [SerializeField] private int _id;

    #endregion

    #region 내부 변수
    // 현재 슬롯에 연결된 장비 데이터
    private CEquipmentDataSO _data;
    #endregion

    private void Start()
    {
        // 초기 데이터 세팅
        SetData(_id);
    }

    #region 데이터 설정
    // ID 기반으로 장비 데이터 로드 및 UI 적용
    public void SetData(int id)
    {
        // SO 데이터 조회
        _data = CSOManager.Instance.SuchData<CEquipmentDataSO>(
            CDataArraySO.EDataType.EquipmentData,
            id
        );

        // 데이터 없으면 종료
        if (_data == null)
        {
            Debug.LogError("데이터 없음: " + id);
            return;
        }

        // 아이콘 적용
        if (_openIcon != null)
            _openIcon.sprite = _data.Image;

        if (_checkIcon != null)
            _checkIcon.sprite = _data.Image;

        // Lock 상태는 시각적으로 구분 (흑백 처리)
        if (_lockIcon != null)
        {
            _lockIcon.sprite = _data.Image;
            _lockIcon.color = Color.black;
        }

        // 스탯 텍스트 적용
        SetStatText();
    }
    #endregion

    #region 내부 함수
    // 장비 스탯 텍스트 생성 및 UI 반영
    private void SetStatText()
    {
        if (_data == null)
            return;

        string text = "";

        // 장비 타입에 따라 하나의 스탯만 표시
        if (_data.AdditionalAttackRatio > 0)
            text = "+" + _data.AdditionalAttackRatio + "%";
        else if (_data.AdditionalHealthRatio > 0)
            text = "+" + _data.AdditionalHealthRatio + "%";
        else if (_data.AdditionalSpeedRatio > 0)
            text = "+" + _data.AdditionalSpeedRatio + "%";

        // 표시할 값 없으면 종료
        if (string.IsNullOrEmpty(text))
            return;

        // Open 상태 텍스트 스타일 적용
        if (_openStatText != null)
        {
            _openStatText.text = text;
            _openStatText.fontSize = 36;
            _openStatText.fontStyle = TMPro.FontStyles.Bold;
            _openStatText.color = Color.white;
        }

        // Check 상태 텍스트
        if (_checkStatText != null)
        {
            _checkStatText.text = text;
            _checkStatText.fontSize = 36;
            _checkStatText.fontStyle = TMPro.FontStyles.Bold;
        }
    }
    #endregion

    #region 외부 호출 함수
    // 현재 장비 데이터 반환
    public CEquipmentDataSO GetData()
    {
        return _data;
    }
    #endregion
}
