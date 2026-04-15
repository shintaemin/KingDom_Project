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

    #region 싱글톤
    public static LTopBar_UI Instance;
    #endregion

    #region 내부 변수
    // 현재 다이아 / 에너지 값 (임시 데이터)
    private int _currentGem = 5000;
    private int _currentEnergy = 15;
    private int _maxEnergy = 15;
    #endregion

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        Instance = this;
    }

    private void Start()
    {
        // 초기 UI 갱신
        RefreshUI();
    }

    #region 외부 호출 함수
    // UI 전체 갱신
    public void RefreshUI()
    {
        // 다이아 표시
        _gemText.text = _currentGem.ToString();

        // 에너지 표시 (현재 / 최대)
        _energyText.text = $"{_currentEnergy} / {_maxEnergy}";

        // 에너지 비율 계산 후 적용
        _energyFill.fillAmount = (float)_currentEnergy / _maxEnergy;
    }

    // 현재 보유 다이아 반환
    public int GetCurrentGem()
    {
        return _currentGem;
    }

    // 다이아 사용 시도
    public bool TryUseGem(int amount)
    {
        // 다이아 부족 시 실패
        if (_currentGem < amount)
            return false;

        // 다이아 차감
        _currentGem -= amount;

        // UI 갱신
        RefreshUI();

        return true;
    }
    #endregion
}
