using TMPro;
using UnityEngine;
using UnityEngine.UI;


#region CRewardPopupPanel
/*
▶ 작성자 류연우

Out_Canvas의 Reward_Popup_Panel에 추가 후 인스펙터에 연결해주면 됩니다.
위 3개를 연결하거나
_~Name들로 설정하면 됩니다.

해금 연결이 되지 않아 OnStatChanged 호출 시 같이 호출하는 방식으로 테스트를 했습니다.

현제 첫번째 해금에 반응하지 않는 버그가 있습니다.
*/
#endregion

public class CRewardPopupPanel : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Image _characterRoot;
    [SerializeField] private Image _statImage;
    [SerializeField] private TextMeshProUGUI _statText;

    [SerializeField] private string _characterRootName = "Character_Root";
    [SerializeField] private string _statImageName = "Stat_Image";
    [SerializeField] private string _statTextName = "Stat_Text";
    #endregion

    #region 내부 변수
    private CEquipmentDataSO _currentData;
    private Sprite _attack;
    private Sprite _hp;
    private Sprite _speed;
    #endregion

    void Awake()
    {
        if(_characterRoot == null)
        {
            _characterRoot = transform.Find(_characterRootName)?.GetComponent<Image>();
        }

        if(_statImage == null)
        {
            _statImage = transform.Find(_statImageName)?.GetComponent<Image>();
        }

        if(_statText == null)
        {
            _statText = transform.Find(_statTextName)?.GetComponent<TextMeshProUGUI>();
        }


        if (_characterRoot.IsNull("_characterRoot") ||
            _statImage.IsNull("_statImage") ||
            _statText.IsNull("_statText")
                )
        {
            return;
        }
        _attack = Resources.Load<Sprite>(CGSSLoader.Sprite_PATH + "/attack");
        _hp = Resources.Load<Sprite>(CGSSLoader.Sprite_PATH + "/hp");
        _speed = Resources.Load<Sprite>(CGSSLoader.Sprite_PATH + "/speed");

        // 이벤트 구독
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnEquipmentUnLock += UpdateUI;
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        
    }
    private void UpdateUI(int ID)
    {
        _currentData = CSOManager.Instance[CDataArraySO.EDataType.EquipmentData][ID] as CEquipmentDataSO;

        Debug.Log($"UpdateUI  ID : {ID}");

        var image = _currentData.Image;

        var type = _currentData.BonusType;
        Sprite stateImage = type switch
        {
            CEquipmentDataSO.EBonusType.Attak => _attack,
            CEquipmentDataSO.EBonusType.Health => _hp,
            CEquipmentDataSO.EBonusType.MoveSpeed => _speed,
            _ => _attack
        };

        var value = _currentData.BonusAmount;

        _characterRoot.sprite = image;
        // 이미지의 비율을 원본으로 변경하고 높이값을 570으로 만든다.

        _statImage.sprite = stateImage;
        _statText.text = $"+ {value}%";
    }
    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (CPlayerDataManager.Instance != null)
            CPlayerDataManager.Instance.OnEquipmentUnLock -= UpdateUI;
    }
}
