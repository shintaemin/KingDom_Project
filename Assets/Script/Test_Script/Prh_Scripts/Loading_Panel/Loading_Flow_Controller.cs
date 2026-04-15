using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading_Flow_Controller : MonoBehaviour
{
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

    private void Start()
    {
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        // 처음 상태
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

        // 3 → 씬 이동
        yield return new WaitForSeconds(_time03);


        // 게임씬으로 이동 (씬 이름은 인스펙터에서 설정 가능)
        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.LoadScene(ESceneLoadType.TestLobby);
        }
    }
}
