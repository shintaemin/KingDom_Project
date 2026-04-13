using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 옷 오브젝트
/*
 ▶ 할일
  - 옷이 갖고있을 기본 정보
  - 오브젝트를 켜고 끌수 있도록 정보를 외부에서 확인할 수 있도록 작업
*/
#endregion


public class Cloth_Object : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private int _id = 0;
    #endregion

    #region 프로퍼티
    public int GetId => _id;
    #endregion
}
