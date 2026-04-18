using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 무기 보유 기반 스탯 UI
/*
 ▶ 할일
  - 무기 해금 개수에 따라 공격력 / 체력 증가 수치 표시
  - 플레이어 스탯 변경 시 UI 자동 갱신

 ▶ 흐름
  1. OnEnable → UI 갱신 + 이벤트 등록
  2. 스탯 변경 이벤트 발생 → UpdateUI 호출
  3. OnDisable → 이벤트 해제

 ※ 참고사항
  - 0.5% 증가

  - 박라희
*/
#endregion
public class WeaponStatText_UI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _hpText;
    #endregion

    void OnEnable()
    {
        if (CPlayerDataManager.Instance == null)
            return;

        UpdateUI();
        CPlayerDataManager.Instance.OnStatChanged += UpdateUI;
    }

    void OnDisable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        var player = CPlayerDataManager.Instance;
        if (player == null)
            return;

        float value = player.UnLockedWeaponCount * 0.5f;

        _attackText.text = $"{value:0.0} %";
        _hpText.text = $"{value:0.0} %";
    }
}
