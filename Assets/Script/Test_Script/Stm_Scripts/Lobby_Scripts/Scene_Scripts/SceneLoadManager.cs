using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region 씬 로드 매니저
/*
 ▶ 할일
  - 씬 로드를 담당
  - 열거형을 통해 동일한 이름의 씬을 재생 하는 방식

    - 작업자 : 신태민 - 
*/
#endregion

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    #region 인스펙터
    [SerializeField] private FadeSystem _fadeSystme;
    [SerializeField] private ESceneLoadType _remainSceneType;
    [SerializeField] private float _waitTime = 2.5f;
    [SerializeField] private float _fadeTime = 0.5f;
    #endregion

    #region 내부 변수
    private Coroutine _loadSceneCo;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if(_fadeSystme == null)
        {
            if(!transform.GetChild(0).TryGetComponent<FadeSystem>(out _fadeSystme))
            {
                Debug.LogWarning($"페이드 시스템이없어 페이드 진행불가");
            }
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        if (_loadSceneCo != null)
        {
            StopCoroutine(_loadSceneCo);
        }

        _loadSceneCo = null;
    }

    private void SetBgm(ESceneLoadType loadType)
    {
        if (SoundManager.Instance == null)
        {
            return;
        }

        EBgmType bgmType = EBgmType.None;

        switch(loadType) // 내부 case 부분 타입만 빌드시 변경
        {
            case ESceneLoadType.TestScene3:
                bgmType = EBgmType.Ingame_1;
                break;
            case ESceneLoadType.TestLobby:
                bgmType = EBgmType.Lobby_1;
                break;
            case ESceneLoadType.TestGame:
                bgmType = EBgmType.Ingame_1;
                break;
        }

        SoundManager.Instance.BgmPlay(bgmType);
    }

    private IEnumerator CoLoadScene(ESceneLoadType loadType)
    {
        float time = _fadeTime;
        if(_fadeSystme != null)
        {
            _fadeSystme.SetActiveFade(true);
            _fadeSystme.Fade(0, 1, time);
            yield return new WaitForSeconds(time);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(loadType.ToString());

        while(!operation.isDone)
        {
            yield return null;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        SetBgm(loadType);

        Time.timeScale = 0f;
        
        yield return new WaitForSecondsRealtime(_waitTime);

        if (_fadeSystme != null)
        {
            _fadeSystme.Fade(1, 0, time);
            yield return new WaitForSecondsRealtime(time);
            _fadeSystme.SetActiveFade(false);
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;

        _loadSceneCo = null;
    }

    #region 외부 호출 함수
    public void LoadScene(ESceneLoadType loadType)
    {
        if (_loadSceneCo != null)
        {
            StopCoroutine(_loadSceneCo);
            _loadSceneCo = null;
        }

        _remainSceneType = loadType;
        _loadSceneCo = StartCoroutine(CoLoadScene(_remainSceneType));
    }

    // 외부에서 현재씬에따른 작업을 위해
    // 액션을 사용해도 좋지만 어떤 작업을 어떻게할지 어느정도로 이벤트를 사용할지 현재는 알수없어 일단은 확인이 가능하도록
    public ESceneLoadType GetRemainScene => _remainSceneType;

    // 페이드를 사용하려면 인게임매니저를 통해서 받아서 사용하도록
    public FadeSystem GetFadeSystem => _fadeSystme;
    #endregion
}
