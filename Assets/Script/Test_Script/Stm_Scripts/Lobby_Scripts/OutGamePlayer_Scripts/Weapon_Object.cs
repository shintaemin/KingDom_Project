using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion


public class Weapon_Object : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private int _id = 0;
    #endregion

    #region 프로퍼티
    public int GetId => _id;
    #endregion
}
