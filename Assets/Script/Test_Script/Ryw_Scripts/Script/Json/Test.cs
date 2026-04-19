using UnityEngine;


#region Test
/*
▶ 작성자 류연우
*/
#endregion

public class Test : MonoBehaviour
{
    #region 인스펙터

    #endregion

    #region 내부 변수

    #endregion

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            int key = (int)KeyCode.Alpha1;
            if (Input.GetKeyDown((KeyCode)key + i))
            {
                CPlayerDataManager.Instance.UnLockEquipmentDic(i);
            }
        }
    }
}
