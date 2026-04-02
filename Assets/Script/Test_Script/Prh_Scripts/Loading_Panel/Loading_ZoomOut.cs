using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 로딩 줌아웃 연출
/*
 ▶ 할일
  - UI 오브젝트를 크게 시작해서 점점 작아지면서 중앙으로 이동
  - 위치와 스케일을 동시에 보간하여 자연스럽게 연출
  - 자동 재생 옵션으로 시작 시 실행 가능

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
        // 컴포넌트 자동 연결 (에디터에서 Reset 눌렀을 때 실행)
        _targetRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        // 자동 실행 옵션이 켜져있으면 연출 시작
        if (_playOnStart)
        {
            Play();
        }
    }

    #region 외부 호출 함수
    // 외부에서 호출하여 연출 실행
    public void Play()
    {
        // 기존 실행 중인 코루틴 정지
        StopAllCoroutines();
        // 새 연출 시작
        StartCoroutine(CoZoomOut());
    }
    #endregion

    // 줌아웃 연출 코루틴
    private IEnumerator CoZoomOut()
    {
        // 대상이 없으면 실행 중지
        if (_targetRect == null)
            yield break;

        // 시작 상태 세팅
        // 시작 위치
        _targetRect.anchoredPosition = _startAnchoredPos;
        // 시작 크기
        _targetRect.localScale = _startScale;

        // 경과 시간
        float elapsed = 0f;

        // 지속시간 동안 반복
        while (elapsed < _duration)
        {
            // 시간 증가
            elapsed += Time.deltaTime;

            // 진행률 (0 ~ 1)
            float t = Mathf.Clamp01(elapsed / _duration);

            // 위치 보간 (시작 → 끝)
            _targetRect.anchoredPosition =
                Vector2.Lerp(_startAnchoredPos, _endAnchoredPos, t);

            // 스케일 보간 (크게 → 작게)
            _targetRect.localScale =
                Vector3.Lerp(_startScale, _endScale, t);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 마지막 정확한 값 적용
        _targetRect.anchoredPosition = _endAnchoredPos;
        _targetRect.localScale = _endScale;
    }
}
