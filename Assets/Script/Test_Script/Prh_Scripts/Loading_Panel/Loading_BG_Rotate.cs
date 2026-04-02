using UnityEngine;

#region 로딩 배경 회전
/*
 ▶ 할일
  - 로딩 화면의 배경 이미지를 회전시킴.
  - RectTransform을 사용하여 UI 기준으로 회전 처리

  - 박라희
*/
#endregion

public class Loading_BG_Rotate : MonoBehaviour
{
    #region 인스펙터
    [Header("회전 속도")]
    [SerializeField] private float _rotateSpeed = 5f;
    #endregion

    #region 내부 변수
    private RectTransform _rectTransform;
    #endregion

    private void Awake()
    {
        // RectTransform 캐싱
        if (!TryGetComponent<RectTransform>(out _rectTransform))
        {
            Debug.LogWarning("[Loading_BG_Rotate] : RectTransform 캐싱 실패");
        }
    }

    private void Update()
    {
        RotateBackground();
    }

    // 배경 회전 처리
    private void RotateBackground()
    {
        if (_rectTransform == null)
        {
            return;
        }

        _rectTransform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
    }

}
