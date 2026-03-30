using UnityEngine;

[System.Serializable]
public class ClipData
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ESfxType _type;

    public AudioClip GetSfxClip => _clip;
    public ESfxType GetSfxType => _type;
}
