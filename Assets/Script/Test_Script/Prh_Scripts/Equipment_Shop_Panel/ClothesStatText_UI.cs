using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 의상 보유 수 기반 스탯 UI
/*
 ▶ 할일
  - 의상 해금 개수에 따라 체력 / 이동속도 증가 수치 표시
  - 플레이어 스탯 변경 시 UI 자동 갱신

 ▶ 흐름
  1. OnEnable → UI 갱신 + 이벤트 등록
  2. 스탯 변경 이벤트 발생 → UpdateUI 호출
  3. OnDisable → 이벤트 해제

 ※ 참고사항
  - 의상 0.5% 증가
*/
#endregion

public class ClothesStatText_UI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _speedText;
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

        float value = player.UnLockedClothesCount* 0.5f;

        _hpText.text = $"{value:0.0} %";
        _speedText.text = $"{value:0.0} %";
    }
}
