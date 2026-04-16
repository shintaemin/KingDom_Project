#region IHPBar
/*
▶ 작성자 류연우

HP UI를 위한 인터페이스.
지금은 CHPBar 하나이지만 여러개여도 이건 하나만 쓴다.
*/
#endregion

using System;
using UnityEngine;

public interface IHPBar
{
    // 체력이 바뀔 때 인보크
    public event Action<float> OnHealthChanged;

    // 위치가 바뀔 때 인보크
    public event Action<Vector3> OnPositionChanged;
}
