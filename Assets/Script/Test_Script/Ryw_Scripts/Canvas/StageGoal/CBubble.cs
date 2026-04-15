using TMPro;
using UnityEngine;
using UnityEngine.UI;


#region CBubble
/*
▶ 작성자 류연우
*/
#endregion

public class CBubble : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    #endregion

    #region 내부 변수

    #endregion

    public Image Image => _image;

    void Awake()
    {
        if (_image.IsNull("_image") || _textMeshProUGUI.IsNull("_textMeshProUGUI"))
        {
            return;
        }
    }
    public void SetImage(Sprite image)
    {
        _image.sprite = image;
    }

    public void SetText(string text, Color color)
    {
        _textMeshProUGUI.text = text;
        _textMeshProUGUI.color = color;
    }
}
