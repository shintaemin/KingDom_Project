using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CouponSystem : MonoBehaviour
{
    #region 인스펙터
    [Header("쿠폰 패널, 배경")]
    [SerializeField] private GameObject _couponPanel;
    [SerializeField] private GameObject _bgImage;

    [Header("UI")]
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _text;
    #endregion

    #region 내부 변수
    private string _coupon = "6TEAM";
    private Coroutine _co;
    #endregion

    void Start()
    {
        _couponPanel.SetActive(false);
        _bgImage.SetActive(false);
        _resultPanel.SetActive(false);
    }

    public void OnClickCouponButton()
    {
        _couponPanel.SetActive(true);
        _bgImage.SetActive(true);
        _resultPanel.SetActive(false);
        _text.color = Color.white;
        _inputField.text = "";
    }

    public void OnClickExit()
    {
        _couponPanel.SetActive(false);
        _bgImage.SetActive(false);
        _resultPanel.SetActive(false);
    }

    public void OnClickCheck()
    {
        string inputText = _inputField.text;

        if (inputText == _coupon)
        {
            _coupon = null;
            _text.color = Color.green;
            _text.text = "10000 다이아 획득!";
            if (CPlayerDataManager.Instance != null)
            {
                CPlayerDataManager.Instance.Gem = 10000;
            }
            _resultPanel.SetActive(true);

            // 재화 추가

            if (_co == null)
            {
                _co = StartCoroutine(CoTextRoutine());
            }
        }

        else
        {
            _text.color = Color.red;
            _text.text = "잘못된 쿠폰 번호입니다.";
            _resultPanel.SetActive(true);

            if (_co == null)
            {
                _co = StartCoroutine(CoTextRoutine());
            }
        }
    }

    private IEnumerator CoTextRoutine()
    {
        yield return new WaitForSeconds(1f);

        _resultPanel.SetActive(false);
        _text.color = Color.white;
        _co = null;
    }
}