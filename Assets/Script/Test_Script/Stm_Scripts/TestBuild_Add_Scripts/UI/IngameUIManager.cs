using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인게임 UI 매니저
/*
 ▶ 할일
  - 현재 씬에 매니저를 구독하고 UI를 켜고 끌수 있도록 작업
*/
#endregion


public class IngameUIManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private IngameManager _gm;
    [SerializeField] private GameObject _stagerPanel;
    [SerializeField] private GameObject _victoryPanel;
    #endregion

    #region

    #endregion

    private void Awake()
    {
        if (_gm == null)
        {
            _gm = FindFirstObjectByType<IngameManager>();
        }
        if (_stagerPanel == null)
        {
            Debug.LogWarning($"[IngameUIManager] : 스테이지 판넬 없음");
            return;
        }
        if (_victoryPanel == null)
        {
            Debug.LogWarning($"[IngameUIManager] : 성공 판넬 없음");
            return;
        }
    }

    private void Start()
    {
        if (_gm == null)
        {
            _gm = FindFirstObjectByType<IngameManager>();
        }

        _gm.OnGameEnd += SetGameEndUI;
        _stagerPanel.SetActive(true);
    }

    private void OnDisable()
    {
        if (_gm != null)
        {
            _gm.OnGameEnd -= SetGameEndUI;
        }
    }

    private void SetGameEndUI(EMissionAnswer answer)
    {
        _stagerPanel.SetActive(false);
        _victoryPanel.SetActive(true);
    }

    #region 외부 호출 함수
    public void GetKillCountUI(int remain, int target)
    {
        if (_stagerPanel == null)
        {
            return;
        }

        if (_stagerPanel.TryGetComponent<Ingame_KillCount_UI>(out Ingame_KillCount_UI killUI))
        {
            killUI.SetKillCountTextUpdata(remain, target);
        }
    }
    #endregion
}
