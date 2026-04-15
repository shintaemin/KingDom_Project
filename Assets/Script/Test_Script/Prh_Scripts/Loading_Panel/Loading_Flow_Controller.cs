using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region 로딩 단계 흐름 제어
/*
 ▶ 할일
  - 로딩 UI를 단계별로 순차 전환 (1 → 2 → 3)
  - 각 단계별 지정된 시간만큼 대기 후 다음 단계로 이동
  - 마지막 단계 종료 후 다음 씬으로 전환

 ※ 참고사항
  - 각 단계는 GameObject 활성/비활성으로 제어
  - 단계별 시간은 _time01 ~ _time03 값으로 설정
  - 씬 이름은 인스펙터에서 설정 (_nextSceneName)

  - 박라희
*/
#endregion

public class Loading_Flow_Controller : MonoBehaviour
{
    #region 인스펙터
    [Header("로딩 단계")]
    [SerializeField] private GameObject _loading01;
    [SerializeField] private GameObject _loading02;
    [SerializeField] private GameObject _loading03;

    [Header("각 단계 시간")]
    [SerializeField] private float _time01 = 1.5f;
    [SerializeField] private float _time02 = 2.5f;
    [SerializeField] private float _time03 = 2f;

    [Header("다음 씬 이름")]
    [SerializeField] private string _nextSceneName;
    #endregion

    private void Start()
    {
        StartCoroutine(CoFlow());
    }

    #region 내부 코루틴
    // 로딩 단계 순차 진행 처리
    private IEnumerator CoFlow()
    {
        // 초기 상태 (1단계 활성화)
        _loading01.SetActive(true);
        _loading02.SetActive(false);
        _loading03.SetActive(false);

        // 1 → 2
        yield return new WaitForSeconds(_time01);
        _loading01.SetActive(false);
        _loading02.SetActive(true);

        // 2 → 3
        yield return new WaitForSeconds(_time02);
        _loading02.SetActive(false);
        _loading03.SetActive(true);

        // 3단계 유지 후 씬 이동 준비
        yield return new WaitForSeconds(_time03);

        // 다음 게임씬으로 이동 (씬 이름은 인스펙터에서 설정 가능)
        // SceneManager.LoadScene(_nextSceneName);
    }
    #endregion
}
