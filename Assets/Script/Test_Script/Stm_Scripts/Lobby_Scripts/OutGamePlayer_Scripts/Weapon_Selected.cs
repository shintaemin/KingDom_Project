using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 무기 셀렉티드
/*
 ▶ 할일
  - 리스트에 등록한 무기 id 를 확인하고 해당되는 id 에맞는 무기를 들도록 작업
*/
#endregion


public class Weapon_Selected : MonoBehaviour
{
    #region 인스펙터 
    [SerializeField] private List<Weapon_Object> _weapons = new List<Weapon_Object>();
    [SerializeField] private GameObject _currentWeapon;
    #endregion

    #region 내부 변수
    private readonly Dictionary<int, Weapon_Object> _weaponDic = new Dictionary<int, Weapon_Object>();
    #endregion

    private void Awake()
    {
        InitListToDic();
        
    }

    private void Start()
    {
        if (CPlayerDataManager.Instance != null)
        {
            int id = CPlayerDataManager.Instance.CurrentWeaponID;
            SetWeapon(id);
        }
    }

    private void OnEnable()
    {
        if (CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.OnStatChanged += ChangeWeapon;
        }
    }

    private void OnDisable()
    {
        if (CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.OnStatChanged -= ChangeWeapon;
        }
    }

    private void InitListToDic()
    {
        if (_weapons.Count == 0)
        {
            return;
        }

        _weaponDic.Clear();

        foreach (Weapon_Object obj in _weapons)
        {
            int id = obj.GetId;

            if (_weaponDic.ContainsKey(id))
            {
                continue;
            }

            _weaponDic.Add(id, obj);
            obj.gameObject.SetActive(false);
        }
    }

    // 외부에서 무기를 변경할 수 있도록 지정
    private void SetWeapon(int id)
    {
        if (!_weaponDic.ContainsKey(id))
        {
            return;
        }
        if (_currentWeapon != null)
        {
            _currentWeapon.SetActive(false);
        }

        Weapon_Object obj = _weaponDic[id];
        _currentWeapon = obj.gameObject;
        _currentWeapon.SetActive(true);
    }

    #region 외부 호출 함수
    // 외부에서 무기를 변경할 수 있도록 지정
    public void ChangeWeapon()
    {
        if (CPlayerDataManager.Instance != null)
        {
            int id = CPlayerDataManager.Instance.CurrentWeaponID;
            SetWeapon(id);
        }
    }

    // 외부 확인할 가능성이 있어서 혹시몰라 작업
    public GameObject GetWeapon => _currentWeapon;
    #endregion
}
