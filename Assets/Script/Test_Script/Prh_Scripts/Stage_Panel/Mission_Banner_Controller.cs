using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 미션 배너 연출
/*
 ▶ 할일
  - 좌 / 우 UI 그룹이 서로 중앙으로 이동하여 만나는 연출
  - 잠깐 정지 후 같은 방향으로 계속 이동하며 화면 밖으로 사라짐
  - 위치 보간(Lerp)을 사용하여 자연스럽게 이동 처리

  - 박라희
 */
#endregion


public class Mission_Banner_Controller : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private RectTransform _leftGroup;
    [SerializeField] private RectTransform _rightGroup;

    [Header("시작 / 중앙 위치")]
    [SerializeField] private Vector2 _leftStart;
    [SerializeField] private Vector2 _leftEnd;

    [SerializeField] private Vector2 _rightStart;
    [SerializeField] private Vector2 _rightEnd;

    [Header("시간 설정")]
    // 중앙까지 이동 시간
    [SerializeField] private float _moveToMeetTime = 0.35f;
    // 중앙에서 유지 시간
    [SerializeField] private float _holdTime = 0.08f;
    // 화면 밖으로 이동 시간
    [SerializeField] private float _moveOutTime = 0.35f;

    [Header("이동 거리")]
    [SerializeField] private float _continueDistance = 1400f;
    #endregion

    private void OnEnable()
    {
        // 활성화되면 시작
        StartCoroutine(CoPlay());
    }

    private IEnumerator CoPlay()
    {
        // 시작 위치로 세팅
        _leftGroup.anchoredPosition = _leftStart;
        _rightGroup.anchoredPosition = _rightStart;

        // 1. 시작 위치 -> 중앙 위치로 만나는 위치
        yield return CoMovePair(_leftStart, _leftEnd, _rightStart, _rightEnd, _moveToMeetTime);

        // 2. 중앙에서 잠깐 유지
        yield return new WaitForSeconds(_holdTime);

        // 3. 같은 방향으로 계속 진행할 목표 계산
        Vector2 leftDir = (_leftEnd - _leftStart).normalized;
        Vector2 rightDir = (_rightEnd - _rightStart).normalized;

        // 4. 중앙 이후 계속 이동할 위치 계산
        Vector2 leftOut = _leftEnd + leftDir * _continueDistance;
        Vector2 rightOut = _rightEnd + rightDir * _continueDistance;

        // 5. 같은 방향으로 계속 이동하여 화면 밖으로 사라짐
        yield return CoMovePair(_leftEnd, leftOut, _rightEnd, rightOut, _moveOutTime);
    }

    // 좌우 배너를 동시에 이동 함수
    private IEnumerator CoMovePair(Vector2 leftFrom, Vector2 leftTo, Vector2 rightFrom, Vector2 rightTo, float duration)
    {
        // 경과 시간
        float time = 0f;

        // duration 동안 반복
        while (time < duration)
        {
            // 시간 증가
            time += Time.deltaTime;
            // 진행률 0~1
            float t = Mathf.Clamp01(time / duration);

            // 위치 보간 (시작 -> 목표)
            _leftGroup.anchoredPosition = Vector2.Lerp(leftFrom, leftTo, t);
            _rightGroup.anchoredPosition = Vector2.Lerp(rightFrom, rightTo, t);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 마지막 정확한 위치 적용 
        _leftGroup.anchoredPosition = leftTo;
        _rightGroup.anchoredPosition = rightTo;
    }
}
