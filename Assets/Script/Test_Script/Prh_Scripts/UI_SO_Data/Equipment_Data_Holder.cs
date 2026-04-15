using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 장비 데이터 전역 관리
/*
 ▶ 역할
  - 현재 선택된 장비(무기 / 의상) 데이터를 전역으로 저장
  - 씬이 변경되어도 데이터가 유지되도록 관리

 ※ 구조 의도
  - 싱글톤 패턴을 사용하여 어디서든 접근 가능하도록 구성
  - DontDestroyOnLoad를 통해 씬 전환 시에도 객체 유지

 ▶ 사용 목적
  - 로비 → 게임씬 이동 시 장비 정보 유지
  - 여러 시스템(UI, 전투 등)에서 동일한 장비 데이터 참조

 ※ 동작 흐름
  1. Awake에서 싱글톤 인스턴스 설정
  2. 기존 인스턴스가 있으면 중복 객체 제거
  3. 최초 객체는 DontDestroyOnLoad로 유지

 ※ 참고사항
  - currentWeapon / currentClothes는 현재 장착 상태를 의미
  - 외부에서 값을 변경하면 전체 시스템에 즉시 반영됨

  - 박라희
*/
#endregion

public class Equipment_Data_Holder : MonoBehaviour
{
    #region 싱글톤
    // 전역 접근용 인스턴스
    public static Equipment_Data_Holder Instance;
    #endregion

    #region 인스펙터
    [Header("현재 선택된 장비")]
    public CEquipmentDataSO currentWeapon;
    public CEquipmentDataSO currentClothes;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 씬이 바뀌어도 유지
        DontDestroyOnLoad(gameObject);
    }
}