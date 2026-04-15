using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region 능력 슬롯 데이터 및 UI 처리
/*
 ▶ 할일
  - 능력 슬롯의 레벨 / 값 / 가격 / 아이콘 UI 관리
  - 업그레이드 버튼 클릭 시 데이터 갱신 및 연출 처리

 ▶ 기능
  - 초기화(Init) : 데이터 세팅 및 기본 상태 설정
  - 업그레이드(OnClickUpgrade) : 레벨 증가 + 값 증가 + 연출 실행
  - UI 갱신(UpdateUI) : 현재 상태를 화면에 반영

 ※ 참고사항
  - 레벨에 따라 가격과 아이콘이 변경됨
  - 아이콘은 일정 레벨 구간마다 교체 (10레벨 단위)

  - 박라희
*/
#endregion

public class AbilitySlot_Data : MonoBehaviour
{
    #region 인스펙터
    [Header("연출")]
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private Transform _spawnPoint;

    [Header("UI")]
    [SerializeField] public Image icon;
    [SerializeField] public TMP_Text levelText;
    [SerializeField] public TMP_Text valueText;
    [SerializeField] public TMP_Text priceText;
    [SerializeField] public Animator effectAnimator;
    #endregion

    #region 내부 변수
    // 현재 레벨
    private int _level = 1;

    // 누적 능력값
    private int _value;

    // 능력 데이터 (SO)
    private CAbilityDataSO _data;
    #endregion

    #region 초기화
    // 슬롯 초기화 (데이터 연결)
    public void Init(CAbilityDataSO so)
    {
        _data = so;

        // 기본 상태 설정
        _level = 1;
        _value = 0;

        // UI 갱신
        UpdateUI();
    }
    #endregion

    #region 외부 호출 함수
    // 업그레이드 버튼 클릭 처리
    public void OnClickUpgrade()
    {
        // 최대 레벨 도달 시 업그레이드 불가
        if (_level >= _data.Capacity)
            return;

        // 레벨 증가
        _level++;

        // 능력값 증가
        _value += _data.Val;

        // 업그레이드 애니메이션 실행
        if (effectAnimator != null)
        {
            effectAnimator.Play("UpgradeEffect", -1, 0f);
        }

        // 파티클 사운드
        SoundManager.Instance.SFXPlay(ESfxType.Upgrade_Status);

        // 파티클 연출 (월드 기준)
        Instantiate(_particlePrefab, _spawnPoint.position, Quaternion.identity);

        // 파티클 연출 (UI 기준 - 자식으로 생성)
        GameObject obj = Instantiate(_particlePrefab, transform);
        obj.transform.localPosition = _spawnPoint.localPosition;

        // UI 갱신
        UpdateUI();
    }
    #endregion

    #region 내부 함수
    // 현재 상태를 UI에 반영
    private void UpdateUI()
    {
        // 레벨 표시
        levelText.text = "LV. " + _level;

        // 능력값 표시
        valueText.text = "+" + _value;

        // 가격 표시 (레벨 기반 배열 참조)
        if (_data.PriceArr != null && _level - 1 < _data.PriceArr.Length)
            priceText.text = _data.PriceArr[_level - 1].ToString();
        else
            priceText.text = "-";

        // 아이콘 변경 (레벨 구간별)
        if (_data.IconArr != null && _data.IconArr.Length > 0)
        {
            // 10레벨 단위로 아이콘 변경
            int iconIndex = (_level - 1) / 10;

            // 배열 범위 보호
            iconIndex = Mathf.Clamp(iconIndex, 0, _data.IconArr.Length - 1);

            icon.sprite = _data.IconArr[iconIndex];
        }
    }
    #endregion
}
