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
  - Sprite 사용 (Image 컴포넌트)

 ▶ 흐름
  1. SetData(id) 호출
  2. CSOManager에서 ID로 데이터 가져오기
  3. 이미지 / 텍스트 UI 반영
  4. 선택 상태는 기본 false

  - 박라희
*/
#endregion

public class Equipment_Slot : MonoBehaviour
{
    #region 인스펙터
    [Header("UI 연결")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private GameObject checkMark;
    #endregion

    #region 내부 변수
    private CEquipmentDataSO data;
    #endregion

    #region 데이터 설정
    /*
     ▶ 역할
      - ID를 통해 장비 데이터 가져오기
      - UI에 데이터 반영

     ▶ 매개변수
      - id : 시트에 있는 장비 ID 값
    */
    public void SetData(int id)
    {
        // 데이터 가져오기
        data = CSOManager.Instance.SuchData<CEquipmentDataSO>(
            CDataArraySO.EDataType.EquipmentData,
            id
        );

        // 데이터 없을 경우 예외 처리
        if (data == null)
        {
            Debug.LogError("데이터 없음: " + id);
            return;
        }

        // 이미지 적용 (Sprite 전환 후 사용)
        // icon.sprite = data.Image;

        // 스탯 텍스트 적용
        SetStatText();

        // 기본 선택 상태 해제
        checkMark.SetActive(false);
    }
    #endregion

    #region 스탯 텍스트 처리
    /*
     ▶ 역할
      - 장비 데이터에 따라 텍스트 설정
      - 값이 있는 스탯만 표시
    */
    private void SetStatText()
    {
        // 공격력 % 배율
        if (data.AdditionalAttackRatio > 0)
            statText.text = "+" + data.AdditionalAttackRatio + "% 공격력";

        // 체력 % 배율
        else if (data.AdditionalHealthRatio > 0)
            statText.text = "+" + data.AdditionalHealthRatio + "% 체력";

        // 이동속도 % 배율
        else if (data.AdditionalSpeedRatio > 0)
            statText.text = "+" + data.AdditionalSpeedRatio + "% 이동속도";

        // 추가 공격력
        else if (data.AdditionalAtt > 0)
            statText.text = "+" + data.AdditionalAtt + " 공격력";

        // 아무 스탯도 없을 경우 빈 텍스트 처리
        else
            statText.text = "";
    }
    #endregion

    #region 선택 처리
    /*
     ▶ 역할
      - 슬롯 선택 여부 표시
      - 체크마크 활성/비활성
    */
    public void SetSelected(bool isSelected)
    {
        checkMark.SetActive(isSelected);
    }
    #endregion
}
