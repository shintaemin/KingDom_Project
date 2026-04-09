using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#region 로딩 UI 문구,바 연출
/*
 ▶ 할일
  - 문구 이미지들 중 1개만 랜덤으로 선택하여 표시
  - 선택되지 않은 문구 이미지는 모두 비활성화
  - 문구 이미지는 GameObject 배열로 관리
  - 로딩바 2초 동안 증가 0 -> 1

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

    #region 로딩바
    private IEnumerator CoFillBar()
    {
        // 로딩바 이미지가 없으면 종료
        if (_barImage == null)
            yield break;

        float time = 0f;

        // 시작 시 0 으로 초기화
        _barImage.fillAmount = 0.2f;

        // 반복
        while (time < _duration)
        {
            // 시간 누적
            time += Time.deltaTime;

            float progress = time / _duration;
            // 0.2 → 1 자연스럽게 증가
            float visibleProgress = Mathf.Lerp(0.2f, 1f, progress);

            _barImage.fillAmount = visibleProgress;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 반복 종료 후 1로 보정
        _barImage.fillAmount = 1f;
    }
    #endregion
}