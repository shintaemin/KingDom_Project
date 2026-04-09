using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 장비 탭 UI 관리
/*
 ▶ 할일
  - 상단 탭(무기/옷) 전환
  - 등급 탭(보통/고급/리워드/구매상품) 전환
  - 현재 선택된 탭 등급에 맞는 그룹만 활성화

  - 박라희
*/
#endregion

public class EquipmentTab_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("상단 탭 이미지")]
    [SerializeField] private GameObject _weaponTabOpen;
    [SerializeField] private GameObject _weaponTabLock;
    [SerializeField] private GameObject _clothesTabOpen;
    [SerializeField] private GameObject _clothesTabLock;

    [Header("탭 루트")]
    [SerializeField] private GameObject _weaponAllGroup;
    [SerializeField] private GameObject _clothesAllGroup;

    [Header("무기 등급 그룹")]
    [SerializeField] private GameObject _weaponNormalGroup;
    [SerializeField] private GameObject _weaponRareGroup;
    [SerializeField] private GameObject _weaponRewardGroup;
    [SerializeField] private GameObject _weaponPurchasedGroup;

    [Header("옷 등급 그룹")]
    [SerializeField] private GameObject _clothesNormalGroup;
    [SerializeField] private GameObject _clothesRareGroup;
    [SerializeField] private GameObject _clothesRewardGroup;
    [SerializeField] private GameObject _clothesPurchasedGroup;
    #endregion

    #region 내부 변수
    // 현재 선택된 탭
    private CategoryType _currentCategory = CategoryType.Weapon;
    // 현재 선택된 등급
    private GradeType _currentGrade = GradeType.Normal;
    #endregion

    private enum CategoryType
    {
        Weapon,
        Clothes
    }

    private enum GradeType
    {
        Normal,
        Rare,
        Reward,
        Purchased
    }
    
    private void Start()
    {
        RefreshUI();
    }

    // 현재 선택 기준으로 전체 UI 갱신
    private void RefreshUI()
    {
        // 현재 무기 탭 선택 여부
        bool isWeapon = _currentCategory == CategoryType.Weapon;

        // 상단 탭 이미지 갱신
        if (_weaponTabOpen != null) _weaponTabOpen.SetActive(isWeapon);
        if (_weaponTabLock != null) _weaponTabLock.SetActive(!isWeapon);

        if (_clothesTabOpen != null) _clothesTabOpen.SetActive(!isWeapon);
        if (_clothesTabLock != null) _clothesTabLock.SetActive(isWeapon);

        // 탭 루트 활성화 갱신
        if (_weaponAllGroup != null) _weaponAllGroup.SetActive(isWeapon);
        if (_clothesAllGroup != null) _clothesAllGroup.SetActive(!isWeapon);

        // 모든 등급 그룹 비활성화
        CloseAllGradeGroups();

        // 현재 탭 + 현재 등급 그룹만 활성화, 무기 탭
        if (isWeapon)
        {
            switch (_currentGrade)
            {
                case GradeType.Normal:
                    if (_weaponNormalGroup != null) _weaponNormalGroup.SetActive(true);
                    break;

                case GradeType.Rare:
                    if (_weaponRareGroup != null) _weaponRareGroup.SetActive(true);
                    break;

                case GradeType.Reward:
                    if (_weaponRewardGroup != null) _weaponRewardGroup.SetActive(true);
                    break;

                case GradeType.Purchased:
                    if (_weaponPurchasedGroup != null) _weaponPurchasedGroup.SetActive(true);
                    break;
            }

            return;
        }

        // 현재 선택된 등급에 맞는 옷 그룹 활성화
        switch (_currentGrade)
        {
            case GradeType.Normal:
                if (_clothesNormalGroup != null) _clothesNormalGroup.SetActive(true);
                break;

            case GradeType.Rare:
                if (_clothesRareGroup != null) _clothesRareGroup.SetActive(true);
                break;

            case GradeType.Reward:
                if (_clothesRewardGroup != null) _clothesRewardGroup.SetActive(true);
                break;

            case GradeType.Purchased:
                if (_clothesPurchasedGroup != null) _clothesPurchasedGroup.SetActive(true);
                break;
        }
    }

    // 모든 등급 그룹 비활성화
    private void CloseAllGradeGroups()
    {
        if (_weaponNormalGroup != null) _weaponNormalGroup.SetActive(false);
        if (_weaponRareGroup != null) _weaponRareGroup.SetActive(false);
        if (_weaponRewardGroup != null) _weaponRewardGroup.SetActive(false);
        if (_weaponPurchasedGroup != null) _weaponPurchasedGroup.SetActive(false);

        if (_clothesNormalGroup != null) _clothesNormalGroup.SetActive(false);
        if (_clothesRareGroup != null) _clothesRareGroup.SetActive(false);
        if (_clothesRewardGroup != null) _clothesRewardGroup.SetActive(false);
        if (_clothesPurchasedGroup != null) _clothesPurchasedGroup.SetActive(false);
    }

    // 무기 활성화
    public void OpenWeaponCategory()
    {
        _currentCategory = CategoryType.Weapon;
        RefreshUI();
    }

    // 옷 활성화
    public void OpenClothesCategory()
    {
        _currentCategory = CategoryType.Clothes;
        RefreshUI();
    }

    // 보통 등급 활성화
    public void OpenNormalGrade()
    {
        _currentGrade = GradeType.Normal;
        RefreshUI();
    }

    // 고급 등급 활성화
    public void OpenRareGrade()
    {
        _currentGrade = GradeType.Rare;
        RefreshUI();
    }

    // 리워드 등급 활성화
    public void OpenRewardGrade()
    {
        _currentGrade = GradeType.Reward;
        RefreshUI();
    }

    // 구매상품 등급 활성화
    public void OpenPurchasedGrade()
    {
        _currentGrade = GradeType.Purchased;
        RefreshUI();
    }

}
