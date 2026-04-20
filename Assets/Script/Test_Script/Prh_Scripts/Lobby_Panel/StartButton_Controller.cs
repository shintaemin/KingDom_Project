using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 시작 버튼 제어
/*
 ▶ 할일
  - 스테이지에 따라 Start / Boss 이미지 전환
  - 시작 버튼 클릭 시 게임 씬으로 이동 요청
  - SceneLoadManager를 통해 씬 전환 처리

 ※ 참고사항
  - 버튼 OnClick 이벤트와 연결하여 사용

  - 박라희
*/
#endregion

public class StartButton_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("UI")]
    [SerializeField] private GameObject startImage;
    [SerializeField] private GameObject bossImage;

    [Header("Stage")]
    [SerializeField] private int currentStage = 1;
    #endregion

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        bool isBossStage = currentStage % 10 == 0;

        startImage.SetActive(!isBossStage);
        bossImage.SetActive(isBossStage);
    }

    #region 외부에서 스테이지 변경 시 호출
    public void SetStage(int stage)
    {
        currentStage = stage;
        UpdateUI();
    }
    #endregion

    #region 외부 호출 함수
    public void OnClickStart()
    {
        if (CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.TryUseEnergy(1);
        }

        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.LoadScene(ESceneLoadType.TestGame);
        }
        else
        {
            Debug.LogWarning("SceneLoadManager가 없음");
        }
    }
    #endregion
}
