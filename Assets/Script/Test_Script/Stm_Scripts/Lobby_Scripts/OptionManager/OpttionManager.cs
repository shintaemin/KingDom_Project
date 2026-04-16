using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

#region 옵션 매니저
/*
 ▶ 할일
  - SoundManager 의 SfxToggle 과 연결
  - 카메라의 PostProcessLayer OnOff 되도록 연결

  - 씬이 바뀌어도 값이 유지되도록 하는 방법을 구상
  - 싱글톤으로 쓸정도는 아니라고 판단되며 DonDestroyOnLoad로 적용해보니 버튼에 할당이 안되고있음
*/
#endregion


public class OpttionManager : MonoBehaviour
{
    #region 내부 변수
    private static bool _usePost = true;
    #endregion

    private void Start()
    {
        UpdatePost();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        UpdatePost();
    }

    private void UpdatePost()
    {
        if (Camera.main != null && Camera.main.TryGetComponent<PostProcessLayer>(out var post))
        {
            post.enabled = _usePost;
        }
    }

    #region 외부 호출 함수
    public void SetSFXToggle()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSfxVolumeToggle();
        }
    }

    public void SetBGMToggle()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBgmVolumeToggle();
        }
    }

    public void SetLowQuality()
    {
        _usePost = false;
        UpdatePost();
    }
    public void SetHighQuality()
    {
        _usePost = true;
        UpdatePost();
    }

    public bool GetUsePost => _usePost;
    #endregion
}
