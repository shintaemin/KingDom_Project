using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 옷 선택
/*
 ▶ 할일
  - 
  - 리스트에 담긴 Cloth_Object 의 id 를 확인하고 
*/

#endregion


public class Cloth_Selected : MonoBehaviour
{
    #region 인스펙터 
    [SerializeField] private List<Cloth_Object> _cloths = new List<Cloth_Object>();
    [SerializeField] private GameObject _currentCloth;
    [SerializeField] private int _testId = 0;
    #endregion

    #region 내부 변수
    private readonly Dictionary<int, Cloth_Object> _clothDic = new Dictionary<int, Cloth_Object>();
    #endregion

    private void Start()
    {
        InitListToDic();
        SetCloth(_testId);
    }

    private void InitListToDic()
    {
        if (_cloths.Count == 0)
        {
            return;
        }

        _clothDic.Clear();

        foreach(Cloth_Object obj in _cloths)
        {
            int id = obj.GetId;

            if (_clothDic.ContainsKey(id))
            {
                continue;
            }

            _clothDic.Add(id, obj);
            obj.gameObject.SetActive(false);
        }
    }

    #region 외부 호출 함수
    public void SetCloth(int id)
    {
        if (!_clothDic.ContainsKey(id))
        {
            return;
        }
        if (_currentCloth != null)
        {
            _currentCloth.SetActive(false);
        }

        Cloth_Object obj = _clothDic[id];
        _currentCloth = obj.gameObject;
        _currentCloth.SetActive(true);
    }
    #endregion
}
