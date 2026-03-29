using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region

#endregion

public enum ESfxType
{
    None,
}
public class ClipData
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ESfxType _type;

    public AudioClip GetSfxClip => _clip;
    public ESfxType GetSfxType => _type;
}
