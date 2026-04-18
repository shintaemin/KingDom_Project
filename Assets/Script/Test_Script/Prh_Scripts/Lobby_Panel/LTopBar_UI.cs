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

    private void OnEnable()
    {
        if (CPlayerDataManager.Instance != null)
        {
            RefreshUI();
            CPlayerDataManager.Instance.OnStatChanged += RefreshUI;
        }
    }
    
    private void OnDisable()
    {
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnStatChanged -= RefreshUI;
    }

    public bool TryUseEnergy(int amount)
    {
        return CPlayerDataManager.Instance.TryUseEnergy(amount);
    }


    // UI 전체 갱신
    public void RefreshUI()
    {
        var data = CPlayerDataManager.Instance;

        // 다이아 표시
        _gemText.text = data.Gem.ToString();

        // 에너지
        _energyText.text = $"{data.Energy} / {data.MaxEnergy}";
        _energyFill.fillAmount = (float)data.Energy / data.MaxEnergy;
    }

    #region 외부 호출 함수
    // 현재 보유 다이아 반환
    //public int GetCurrentGem()
    //{
    //    return CPlayerDataManager.Instance.Gem;
    //}

    // 다이아 사용 시도
    public bool TryUseGem(int amount)
    {
        // 데이터 매니저에 사용 요청
        bool result = CPlayerDataManager.Instance.TryUseGem(amount);

        if (result)
            RefreshUI();

        return result;
    }
    #endregion
}
