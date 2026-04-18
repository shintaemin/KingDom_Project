using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameImageUi : MonoBehaviour
{
    #region 인스펙터

    #endregion

    #region 내부 변수
    private int _totalGem = 0;
    #endregion

    private void OnEnable()
    {
        GemParticle.OnGemCollected += GemCollected;
    }

    private void OnDisable()
    {
        GemParticle.OnGemCollected -= GemCollected;
    }

    private void GemCollected(int amount)
    {
        // 잼 수급 텍스트 구현
    }

    private void Awake()
    {
           
    }

    void Update()
    {
        
    }
}
