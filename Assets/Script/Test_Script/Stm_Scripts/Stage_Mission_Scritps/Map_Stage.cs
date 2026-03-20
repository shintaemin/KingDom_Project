using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 맵 스크립트
/*
 ▶ 할일
  - 맵별 인덱스를 지정
*/
#endregion


public class Map_Stage : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private int _index;
    #endregion

    #region

    #endregion

    #region 외부 호출 함수
    // 레벨 매니저가 플레이어의 레벨(스테이지) 데이터에 따른 내보낼 맵을 찾기위함
    public int GetIndex => _index;
    #endregion
}
