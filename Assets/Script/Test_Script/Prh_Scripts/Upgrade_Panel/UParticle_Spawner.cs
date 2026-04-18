using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 업그레이드 파티클
/*
 ▶ 할일
  - 자식 오브젝트에 포함된 모든 ParticleSystem을 캐싱
  - 외부 호출 시 파티클 이펙트를 재생
  - 일정 시간 이후 파티클을 자동으로 정지

 ※ 참고사항
  - Spawn() 호출 시 기존 실행중인 코루틴은 중지 후 재실행
  - 모든 파티클은 동일한 타이밍으로 재생 및 정지 처리

  - 박라희
*/
#endregion


public class UParticle_Spawner : MonoBehaviour
{
    #region 내부 변수
    private ParticleSystem[] _psList;
    #endregion

    void Awake()
    {
        // 자식 포함 모든 ParticleSystem 캐싱
        _psList = GetComponentsInChildren<ParticleSystem>();
    }

    #region 외부 호출 함수
    // 파티클 재생
    public void Spawn()
    {
        Debug.Log("Spawn 호출됨");

        _psList = GetComponentsInChildren<ParticleSystem>(true);

        StopAllCoroutines();
        StartCoroutine(CoPlayEffect());
    }
    #endregion

    #region 내부 함수
    // 파티클 재생 및 일정 시간 후 정지 처리
    IEnumerator CoPlayEffect()
    {
        // 재생
        foreach (var ps in _psList)
        {
            ps.Play();
        }

        yield return new WaitForSeconds(2f);

        foreach (var ps in _psList)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
    #endregion
}
