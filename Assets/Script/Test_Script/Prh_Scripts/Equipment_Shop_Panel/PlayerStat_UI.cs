using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 플레이어 스탯 UI 표시 (이벤트 기반)
/*
 ▶ 할일
  - 플레이어의 공격력 / 체력 / 이동속도 값을 UI에 표시
  - 스탯 변경 이벤트 발생 시 UI 갱신

 ※ 참고사항
  - CPlayerDataManager의 OnStatChanged 이벤트를 구독하여 갱신
  - OnEnable / OnDisable에서 이벤트 등록 및 해제
  - 이동속도는 퍼센트(%) 형태로 변환하여 표시

  - 박라희
*/
#endregion

public class PlayerStat_UI : MonoBehaviour
{
    #region 인스펙터
    [Header("텍스트 연결")]
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _speedText;
    #endregion

    private void Start()
    {
        // 초기 UI 갱신
        if (CPlayerDataManager.Instance != null)
            UpdateUI();
    }

    #region 외부 호출 함수
    // 플레이어 스탯 UI 갱신
    public void UpdateUI()
    {
        var player = CPlayerDataManager.Instance;

        // 데이터 없으면 종료
        if (player == null)
            return;

        try
        {
            // 공격력
            _attackText.text = player.Attack.ToString();

            // 체력
            _hpText.text = player.HP.ToString();

            // 이동속도 퍼센트 변환
            _speedText.text = (player.MoveSpeed * 100f).ToString("F0") + "%";
        }
        catch
        {
            // 초기화 안된 상태 방어
            return;
        }
    }
    #endregion

    #region 이벤트 처리
    private void OnEnable()
    {
        // 이벤트 구독
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged += UpdateUI;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= UpdateUI;
    }
    #endregion
}
