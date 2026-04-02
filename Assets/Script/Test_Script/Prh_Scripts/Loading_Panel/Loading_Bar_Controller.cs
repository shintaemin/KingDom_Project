using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#region 로딩 문구 랜덤 표시
/*
 ▶ 할일
  - 문구 이미지들 중 1개만 랜덤으로 선택하여 표시
  - 선택되지 않은 문구 이미지는 모두 비활성화
  - 문구 이미지는 GameObject 배열로 관리

  - 박라희
  - 로딩바는 추후 인게임씬과 연결할 예정이므로, 로딩바 연출 보류함.
*/
#endregion

public class Loading_Bar_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("문구 이미지 목록")]
    [SerializeField] private GameObject[] _loadingMessageObjects;
    #endregion

    private void Start()
    {
        ShowRandomMessage();
    }
    
    // 랜덤 문구 1개만 활성화
    private void ShowRandomMessage()
    {
        // 배열 체크
        if (_loadingMessageObjects == null || _loadingMessageObjects.Length == 0)
        {
            return;
        }

        // 모든 문구 비활성화
        for (int i = 0; i < _loadingMessageObjects.Length; i++)
        {
            if (_loadingMessageObjects[i] != null)
            {
                _loadingMessageObjects[i].SetActive(false);
            }
        }

        // 랜덤 인덱스 선택
        int randomIndex = Random.Range(0, _loadingMessageObjects.Length);

        // 선택된 문구 활성화
        if (_loadingMessageObjects[randomIndex] != null)
        {
            _loadingMessageObjects[randomIndex].SetActive(true);
        }
    }

}