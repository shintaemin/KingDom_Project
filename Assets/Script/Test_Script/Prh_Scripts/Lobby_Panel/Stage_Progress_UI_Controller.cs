using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region 스테이지 진행 UI 관리
/*
 ▶ 할일
  - 현재 스테이지 기준으로 아이콘 상태(클리어 / 진행중 / 잠금) 표시
  - 현재 스테이지는 강조(크기 + 색상 변경)
  - 스테이지 범위에 따라 UI 그룹 전환

 ▶ 흐름
  1. UpdateStageUI 호출
  2. 모든 아이콘 순회
  3. 현재 스테이지 기준으로 색상 및 크기 설정
  4. UpdateStageGroup 호출 → UI 그룹 전환

  - 박라희
*/
#endregion

public class Stage_Progress_UI_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("스테이지 아이콘")]
    [SerializeField] private List<Image> stageIcons;

    [SerializeField] private StartButton_Controller startButton;

    [SerializeField] private int currentStage = 1;

    [Header("스테이지 그룹")]
    [SerializeField] private GameObject groupAll;
    [SerializeField] private GameObject group1_10;
    [SerializeField] private GameObject group11_20;

    [Header("색상")]
    [SerializeField] private Color clearColor = new Color32(253, 165, 1, 255);
    [SerializeField] private Color currentColor = Color.white;
    [SerializeField] private Color lockColor = new Color32(20, 26, 52, 255);

    [Header("크기")]
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float currentScale = 1.5f;
    #endregion

    void Start()
    {
        if (CPlayerDataManager.Instance != null)
        {
            currentStage = CPlayerDataManager.Instance.CurrentStage;
        }

        UpdateStageUI();
        startButton.SetStage(currentStage);
    }

    #region 외부 호출 함수
    public void SetStage(int stage)
    {
        currentStage = stage;
        UpdateStageUI();

        startButton.SetStage(stage);
    }

    // 스테이지 UI 전체 갱신
    public void UpdateStageUI()
    {
        for (int i = 0; i < stageIcons.Count; i++)
        {
            int stageNumber = i + 1;

            Image icon = stageIcons[i];
            TMP_Text text = icon.GetComponentInChildren<TMP_Text>();
            RectTransform rect = icon.GetComponent<RectTransform>();

            // 기본값
            rect.localScale = Vector3.one * normalScale;

            if (text != null)
                text.color = Color.white;

            // 클리어
            if (stageNumber < currentStage)
            {
                icon.color = clearColor;
            }

            // 현재
            else if (stageNumber == currentStage)
            {
                icon.color = currentColor;

                // 아이콘 확대
                rect.localScale = Vector3.one * currentScale;

                // 텍스트 색상 변경
                if (text != null)
                    text.color = Color.black;
            }

            // 잠금
            else
            {
                icon.color = lockColor;
            }
        }

        // 그룹 전환
        UpdateStageGroup();
    }
    #endregion


    private void UpdateStageGroup()
    {
        // 1~10
        groupAll.SetActive(true);
        group1_10.SetActive(true);
        group11_20.SetActive(false);

        // 11~20
        if (currentStage >= 11)
        {
            group1_10.SetActive(false);
            group11_20.SetActive(true);
        }

        // 21 이상 (전체 그룹 비활성화)
        if (currentStage > 20)
        {
            groupAll.SetActive(false);
        }
    }

}
