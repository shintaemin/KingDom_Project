using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 로비 버튼 사운드 재생
/*
 ▶ 할일
  - 버튼 클릭 시 효과음(SFX) 재생
  - SoundManager를 통해 사운드 출력 요청

 ※ 참고사항
  - SoundManager.Instance가 존재할 경우에만 실행
  - 버튼 이벤트(OnClick)와 연결하여 사용

  - 박라희
*/
#endregion

public class Lobby_Button_Sound : MonoBehaviour
{
    #region 외부 호출 함수
    // 버튼 클릭 사운드 재생
    public void PlaySound()
    {
        // SoundManager 존재 여부 확인
        if (SoundManager.Instance != null)
        {
            // 버튼 클릭 효과음 재생
            SoundManager.Instance.SFXPlay(ESfxType.Touch_Button);
        }
    }
    #endregion
}
