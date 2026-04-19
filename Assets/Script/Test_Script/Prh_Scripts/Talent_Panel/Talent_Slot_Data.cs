using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region 재능 슬롯 데이터 적용
/*
 ▶ 할일
  - 재능 ID를 기반으로 데이터를 가져와 슬롯 UI에 적용
  - 아이콘 및 스탯 텍스트를 UI에 표시
  - 잠금 / 해금 / 선택 상태에서 동일한 정보 유지

 ※ 참고사항
  - CSOManager를 통해 SO 데이터 조회
  - 데이터의 Information 문자열에 Volume 값을 치환하여 사용
  - 동일한 텍스트를 여러 UI(open / check)에 동시에 적용

 ※ 데이터 흐름
  ID → TalentDataSO 조회 → UI (아이콘 / 텍스트) 반영

  - 박라희
*/
#endregion

public class Talent_Slot_Data : MonoBehaviour
{
    #region 인스펙터
    [Header("아이콘")]
    [SerializeField] private Image _lockIcon;
    [SerializeField] private Image _openIcon;
    [SerializeField] private Image _checkIcon;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI _openStatText1;
    [SerializeField] private TextMeshProUGUI _openStatText2;
    [SerializeField] private TextMeshProUGUI _checkStatText1;
    [SerializeField] private TextMeshProUGUI _checkStatText2;

    [Header("ID")]
    [SerializeField] private int _id;
    #endregion

    #region 내부 변수
    // 현재 슬롯에 연결된 재능 데이터
    private CTalentDataSO _data;
    #endregion

    private void Start()
    {
        // 초기 데이터 세팅
        SetData(_id);
    }

    #region 데이터 설정
    // ID 기반으로 데이터 로드 및 UI 적용
    public void SetData(int id)
    {
        // SO 데이터 조회
        _data = CSOManager.Instance.SuchData<CTalentDataSO>(
            CDataArraySO.EDataType.TalentData,
            id
        );

        // 데이터 없으면 에러 로그 출력
        if (_data == null)
        {
            Debug.LogError("재능 데이터 없음: " + id);
            return;
        }

        // 아이콘 적용 (해금 상태 기준)
        if (_openIcon != null)
            _openIcon.sprite = _data.Icon;

        // 스탯 텍스트 적용
        SetStatText();
    }
    #endregion

    #region 내부 함수
    // 스탯 텍스트 생성 및 UI 반영
    private void SetStatText()
    {
        if (_data == null)
            return;

        if (CPlayerDataManager.Instance == null)
            return;

        int level = CPlayerDataManager.Instance.GetCurrentTalentLevel(_id);

        int value = 0;

        if (level > 0)
        {
            value = _data.Basic + _data.Volume * (level - 1);
        }

        // 문자열 포맷: "{}" → 실제 수치
        // string text = _data.Information.Replace("{}", _data.Volume.ToString());
        string text = _data.Information.Replace("{}", value.ToString());

        // 전체 합
        int sum = CPlayerDataManager.Instance.CurrentTalentSum;

        // 해금 상태 텍스트
        if (_openStatText1 != null)
            _openStatText1.text = text;

        if (_openStatText2 != null)
            _openStatText2.text = text;

        // 선택 상태 텍스트
        if (_checkStatText1 != null)
            _checkStatText1.text = text;

        if (_checkStatText2 != null)
            _checkStatText2.text = text;
    }
    #endregion

    #region 외부 호출 함수
    // 현재 슬롯 데이터 반환
    public CTalentDataSO GetData()
    {
        return _data;
    }

    private void OnEnable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        SetStatText();
    }
    #endregion
}
