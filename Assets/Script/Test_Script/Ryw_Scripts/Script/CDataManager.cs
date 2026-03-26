using System.Collections.Generic;
using UnityEngine;


#region CDataManager
/*

*/
#endregion

public class CDataManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private CDataArraySO _dataArraySO;
    #endregion

    #region 내부 변수
    public static CDataManager Instance;

    //private Dictionary

    #endregion

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance != null && Instance != this");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _dataArraySO.IsNull("_dataArraySO");

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
