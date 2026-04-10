using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 장비 데이터 저장
/*
 ▶ 역할
  - 선택된 장비 저장
  - 씬이 바뀌어도 유지

  - 박라희
*/
#endregion

public class Equipment_Data_Holder : MonoBehaviour
{
    public static Equipment_Data_Holder Instance;

    [Header("현재 선택된 장비")]
    public CEquipmentDataSO currentWeapon;
    public CEquipmentDataSO currentClothes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}