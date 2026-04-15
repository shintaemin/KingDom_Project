using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 시작 버튼 제어
/*
 ▶ 할일
  - 시작 버튼 클릭 시 게임 씬으로 이동 요청
  - SceneLoadManager를 통해 씬 전환 처리

 ※ 참고사항
  - SceneLoadManager가 존재할 경우에만 실행
  - 버튼 OnClick 이벤트와 연결하여 사용

  - 박라희
*/
#endregion

public class StartButton_Controller : MonoBehaviour
{
    #region 외부 호출 함수
    public void OnClickStart()
    {
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
