using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#region 로딩 UI 문구,바 연출
/*
 ▶ 할일
  - 문구 이미지들 중 1개만 랜덤으로 선택하여 표시
  - 선택되지 않은 문구 이미지는 모두 비활성화
  - 문구 이미지는 GameObject 배열로 관리
  - 로딩바를 일정 시간 동안 자연스럽게 증가 (0.2 → 1)

※ 참고사항
  - OnEnable 시 문구 갱신 및 로딩바 재생
  - 로딩 시간은 _duration 값으로 제어
  - 로딩바는 fillAmount 기반으로 처리

  - 박라희
*/
#endregion

public class Loading_Bar_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("문구 이미지 목록")]
    [SerializeField] private GameObject[] _loadingMessageObjects;

    [Header("로딩바")]
    [SerializeField] private Image _barImage;

    [Header("로딩 시간")]
    [SerializeField] private float _duration = 2f;
    #endregion

    private void OnEnable()
    {
        // 랜덤 문구 표시
        ShowRandomMessage();
        // 로딩바 채우기
        StartCoroutine(CoFillBar());
    }
    
    // 랜덤 문구 1개만 활성화
    private void ShowRandomMessage()
    {
        // 배열 체크, 배열이 비어있거나 없으면 실행 안함
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

    #region 내부 코루틴
    // 로딩바 fillAmount 증가 처리
    private IEnumerator CoFillBar()
    {
        // 로딩바 이미지가 없으면 종료
        if (_barImage == null)
            yield break;

        float currentTime = 0f;

        // 시작 값 초기화 (최소값 0.2)
        _barImage.fillAmount = 0.2f;

        // 로딩 진행
        while (currentTime < _duration)
        {
            // 시간 누적
            currentTime += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float progress = currentTime / _duration;

            // 0.2 → 1 보간
            float visibleProgress = Mathf.Lerp(0.2f, 1f, progress);

            _barImage.fillAmount = visibleProgress;

            yield return null;
        }

        // 종료 시 1로 보정
        _barImage.fillAmount = 1f;
    }
    #endregion
}