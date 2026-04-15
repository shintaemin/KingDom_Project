using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 로딩 줌아웃 연출
/*
 ▶ 할일
  - UI 오브젝트를 크게 시작해서 점점 작아지면서 중앙으로 이동
  - 위치와 스케일을 동시에 보간하여 자연스럽게 연출
  - 자동 재생 옵션으로 시작 시 실행 가능

※ 참고사항
  - RectTransform 기준으로 위치(anchoredPosition)와 스케일(localScale) 제어
  - 시간 기반 보간(Lerp)으로 부드러운 이동 처리
  - 연출 시간은 _duration 값으로 조절 가능

  - 박라희
*/
#endregion


public class Loading_ZoomOut : MonoBehaviour
{
    #region 인스펙터
    [Header("연출 대상")]
    [SerializeField] private RectTransform _targetRect;

    [Header("시작 위치")]
    [SerializeField] private Vector2 _startAnchoredPos = new Vector2(0f, -220f);

    // 중앙
    [Header("최종 위치")]
    [SerializeField] private Vector2 _endAnchoredPos = Vector2.zero;

    [Header("시작 크기")]
    [SerializeField] private Vector3 _startScale = new Vector3(1.8f, 1.8f, 1f);

    // 원래 크기
    [Header("최종 크기")]
    [SerializeField] private Vector3 _endScale = Vector3.one;

    [Header("연출 시간")]
    [SerializeField] private float _duration = 1.2f;

    [Header("자동 재생")]
    [SerializeField] private bool _playOnStart = true;
    #endregion

    private void Reset()
    {
        // RectTransform 자동 캐싱 (에디터 Reset 시)
        _targetRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        // 자동 실행 옵션 체크
        if (_playOnStart)
        {
            Debug.Log("TimeScale: " + Time.timeScale);

            Play();
        }
    }

    #region 외부 호출 함수
    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(CoZoomOut());
    }
    #endregion

    #region 내부 코루틴
    // 위치 + 스케일 보간을 통한 줌아웃 연출
    public IEnumerator CoZoomOut()
    {
        // 대상이 없으면 실행 중지
        if (_targetRect == null)
            yield break;

        // 시작 상태 설정
        _targetRect.anchoredPosition = _startAnchoredPos;
        _targetRect.localScale = _startScale;

        // 경과 시간 변수
        float currentTime = 0f;

        // 연출 진행
        while (currentTime < _duration)
        {
            // 시간 누적
            currentTime += Time.deltaTime;

            // 진행률 계산 (0 ~ 1)
            float t = Mathf.Clamp01(currentTime / _duration);

            // 위치 보간 (시작 → 끝)
            _targetRect.anchoredPosition =
                Vector2.Lerp(_startAnchoredPos, _endAnchoredPos, t);

            // 스케일 보간 (확대 → 원래 크기)
            _targetRect.localScale =
                Vector3.Lerp(_startScale, _endScale, t);

            yield return null;
        }

        // 최종 값 보정
        _targetRect.anchoredPosition = _endAnchoredPos;
        _targetRect.localScale = _endScale;
    }

    #endregion
}
