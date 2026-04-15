using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 재능 UI 회전 연출
/*
 ▶ 할일
  - UI 오브젝트를 일정 속도로 회전시켜 연출
  - Z축 기준으로 지속적인 회전 처리

 ※ 참고사항
  - Update에서 매 프레임 회전 적용
  - Time.deltaTime을 사용하여 프레임 독립적인 회전 구현

  - 박라희
*/
#endregion

public class Talent_Rotate_UI : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private float _speed = 100f;
    #endregion

    private void Update()
    {
        // Z축 기준 회전
        transform.Rotate(0f, 0f, _speed * Time.deltaTime);
    }
}
