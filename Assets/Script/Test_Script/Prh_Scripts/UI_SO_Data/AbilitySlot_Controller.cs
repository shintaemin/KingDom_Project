using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 능력치 슬롯 초기화 관리
/*
 ▶ 할일
  - 능력(공격 / 체력 / 속도) 슬롯에 데이터 연결
  - 각 슬롯에 대응되는 ScriptableObject를 전달하여 초기화

 ※ 구조 의도
  - Controller는 "데이터 연결 역할"만 담당
  - 실제 UI 처리 및 값 적용은 AbilitySlot_Data에서 수행

 ▶ 흐름
  1. Start 시 각 능력 슬롯 존재 여부 확인
  2. 대응되는 SO 데이터를 전달하여 Init 실행
  3. 슬롯 내부에서 UI 갱신 처리

  - 박라희
*/
#endregion

public class AbilitySlot_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("슬롯 참조")]
    [SerializeField] private AbilitySlot_Data _attackSlot;
    [SerializeField] private AbilitySlot_Data _healthSlot;
    [SerializeField] private AbilitySlot_Data _speedSlot;

    [Header("능력 데이터")]
    [SerializeField] private CAbilityDataSO _attackSO;
    [SerializeField] private CAbilityDataSO _healthSO;
    [SerializeField] private CAbilityDataSO _speedSO;
    #endregion

    private void Start()
    {
        // 각 슬롯에 데이터 전달 및 초기화
        _attackSlot.Init(_attackSO);
        _healthSlot.Init(_healthSO);
        _speedSlot.Init(_speedSO);
    }
}
