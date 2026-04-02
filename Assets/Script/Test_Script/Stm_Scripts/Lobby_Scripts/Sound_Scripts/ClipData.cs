using UnityEngine;

[System.Serializable]
public class ClipData
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ESfxType _sfxType;
    [SerializeField] private EBgmType _bgmType;

    public AudioClip GetClip => _clip;
    public ESfxType GetSfxType => _sfxType;
    public EBgmType GetBgmType => _bgmType;
}
