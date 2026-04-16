using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 사운드 매니저
/*
 ▶ 할일
  - 싱글톤으로 외부에서 ClipData 타입을 통해 사용
  - 클립 데이터를 리스트에 등록하고
  - 딕셔너리를 사용해 타입을 통해 꺼내쓰는 방식

    - 작업자 : 신태민 - 
*/
#endregion


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    #region 인스펙터
    [SerializeField] private AudioSource _bgmAudio;
    [SerializeField] private AudioSource _sfxAudio;
    [SerializeField, Range(0,1)] private float _sfxVolum;
    [SerializeField, Range(0,1)] private float _bgmVolum;
    [SerializeField] private List<ClipData> _bgmRegistry = new List<ClipData>();
    [SerializeField] private List<ClipData> _sfxRegistry = new List<ClipData>();
    [SerializeField] private bool _bgmToggle = true;
    [SerializeField] private bool _sfxToggle = true;
    #endregion

    #region 내부변수
    private readonly Dictionary<ESfxType, List<AudioClip>> _sfxClips = new Dictionary<ESfxType, List<AudioClip>>();
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        AudioSource[] sources = GetComponentsInChildren<AudioSource>();

        if (_bgmAudio == null)
        {
            if (sources[0] != null && !sources[0].TryGetComponent<AudioSource>(out _bgmAudio))
            {
                Debug.LogWarning($"[SoundManager] : Bgm 소스 없음 비지엠 재생 불가");
                return;
            }
        }

        if (_sfxAudio == null)
        {
            if (sources[1] != null && !sources[1].TryGetComponent<AudioSource>(out _sfxAudio))
            {
                Debug.LogWarning($"[SoundManager] : Sfx 소스 없음 효과음 재생 불가");
                return;
            }
        }

        InitSfxClips();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void InitSfxClips()
    {
        if (_sfxAudio == null)
        {
            return;
        }

        if (_sfxClips.Count != 0)
        {
            _sfxClips.Clear();
        }

        foreach (var data in _sfxRegistry)
        {
            if (data == null || data.GetClip == null) continue;

            ESfxType type = data.GetSfxType;

            if (!_sfxClips.ContainsKey(type))
            {
                _sfxClips[type] = new List<AudioClip>();
            }

            AudioClip clip = data.GetClip;

            _sfxClips[type].Add(clip);
        }

        _sfxAudio.volume = _sfxVolum;
        _sfxAudio.playOnAwake = false;
    }

    #region 외부 호출 함수
    #region 효과음 재생 방법
    /*
     열거형란에 Audio 클립의 이름을 넣어본다.
     종류가 많은 클립이라면 random = true 로 했을떄 랜덤재생된다.
    */
    #endregion
    public void SFXPlay(ESfxType type, bool random = false)
    {
        if (_sfxAudio == null)
        {
            return;
        }

        // 타입 미등록 사운드 호출시 호출 될 수 있음
        if (!_sfxClips.TryGetValue(type , out List<AudioClip> clips))
        {
            Debug.Log($"[SoundManager] : {type} 이 미등록 이거나 클립이 없음");
            return;
        }

        int index = random ? Random.Range(0, clips.Count) : 0; 
        _sfxAudio.PlayOneShot(clips[index]);
    }

    public void StopSfx()
    {
        if (_sfxAudio == null)
        {
            return;
        }

        _sfxAudio.Stop();
    }

    public void BgmPlay(EBgmType type)
    {
        if(_bgmAudio == null)
        {
            return;
        }

        for(int i = 0; i < _bgmRegistry.Count; i++)
        {
            if (_bgmRegistry[i] == null)
            {
                continue;
            }

            ClipData data = _bgmRegistry[i];
            EBgmType clipType = data.GetBgmType;

            if(clipType == type)
            {
                AudioClip clip = data.GetClip;
                _bgmAudio.clip = clip;
                break;
            }
        }

        _bgmAudio.playOnAwake = false;
        _bgmAudio.loop = true;
        _bgmAudio.volume = _bgmVolum;
        _bgmAudio.Play();
    }

    public void StopBgm()
    {
        if (_bgmAudio == null)
        {
            return;
        }

        _bgmAudio.Stop();
    }

    public void SetBgmVolumeToggle()
    {
        _bgmToggle = !_bgmToggle;
        _bgmAudio.mute = !_bgmToggle;
    }
    public void SetSfxVolumeToggle()
    {
        _sfxToggle = !_sfxToggle;
        _sfxAudio.mute = !_sfxToggle;
    }

    public bool GetSfxToggle => _sfxToggle;
    public bool GetBgmToggle => _bgmToggle;
    #endregion
}
