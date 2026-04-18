using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 로비 스탯 UI 표시
/*
 ▶ 할일
  - 플레이어의 공격력 / 체력 / 이동속도 값을 UI에 표시
  - 값이 변경될 때만 UI 갱신하여 성능 최적화

 ※ 참고사항
  - 이전 값 캐싱 후 변화가 있을 때만 UpdateUI 호출
  - 이동속도는 퍼센트(%) 형태로 변환하여 표시
  - PlayerDataManager를 통해 데이터 접근

  - 박라희
*/
#endregion

public class LobbyStat_UI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _speedText;
    #endregion

    #region 내부 변수
    // 이전 값 캐싱 (변화 감지용)
    private int _cachedAttack;
    private int _cachedHp;
    private float _cachedMoveSpeed;
    #endregion

    private void Start()
    {
        // 초기값 설정 (강제 갱신 유도)
        _cachedAttack = -1;
        _cachedHp = -1;
        _cachedMoveSpeed = -1f;

        UpdateUI();
    }

    private void Update()
    {
        var player = CPlayerDataManager.Instance;

        // 데이터 없으면 종료
        if (player == null)
            return;

        // 값 변화 없으면 갱신 스킵
        if (player.Attack == _cachedAttack &&
            player.HP == _cachedHp &&
            player.MoveSpeedRatio == _cachedMoveSpeed)
            return;

        // 캐싱 값 갱신
        _cachedAttack = player.Attack;
        _cachedHp = player.HP;
        _cachedMoveSpeed = player.MoveSpeedRatio;

        // UI 갱신
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
            // 공격력 표시
            _attackText.text = player.Attack.ToString();

            // 체력 표시
            _hpText.text = player.HP.ToString();

            // 이동속도 퍼센트 변환 후 표시
            _speedText.text = (player.MoveSpeedRatio * 100f).ToString("F0") + "%";
        }
        catch
        {
            // 초기화 안된 상태 방어
            return;
        }
    }
    #endregion
}
