using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region 상단 UI (에너지 / 다이아)
/*
▶ 할일
 - 보유 다이아 및 에너지 값을 UI에 표시
 - 에너지 비율을 이미지 fillAmount로 시각화
 - 다이아 사용 시 값 갱신 및 UI 반영

※ 참고사항
 - 싱글톤(Instance)으로 외부에서 접근 가능
 - 에너지 fillAmount는 현재값 / 최대값 비율로 계산
 - 현재 값은 임시 데이터 (추후 데이터 매니저 연동 필요)

 - 박라희
*/
#endregion

public class LTopBar_UI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private TextMeshProUGUI _gemText;
    [SerializeField] private TextMeshProUGUI _energyText;
    [SerializeField] private UnityEngine.UI.Image _energyFill;
    #endregion

    public static LTopBar_UI Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshUI();

        CPlayerDataManager.Instance.OnStatChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= RefreshUI;
    }

    
    // UI 전체 갱신
    public void RefreshUI()
    {
        var data = CPlayerDataManager.Instance;

        // 다이아 표시
        _gemText.text = data.Gem.ToString();

        // 에너지
        _energyText.text = $"{data.Energy} / 15";
        _energyFill.fillAmount = (float)data.Energy / 15f;
    }


    // 현재 보유 다이아 반환
    public int GetCurrentGem()
    {
        return CPlayerDataManager.Instance.Gem;
    }

    #region 외부 호출 함수
    public bool TryUseGem(int amount)
    {
        bool result = CPlayerDataManager.Instance.TryUseGem(amount);

        if (result)
            RefreshUI();

        return result;

    }
    #endregion
}
