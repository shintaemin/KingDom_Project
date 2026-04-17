using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndButton : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private float _waitTime = 5f;
    [SerializeField] private float _endWaitTime = 2f;
    [SerializeField] private CInGameCanvas _uiCanvas;
    [SerializeField] private Button _endButton;
    [SerializeField] private GemUIUpdate _gemUI;
    #endregion

    #region 내부 변수
    private Coroutine _gemAnimCo;
    #endregion
    public void GameToLobbyButton()
    {
        if (_gemAnimCo != null)
        {
            StopCoroutine(_gemAnimCo);
        }

        _endButton.interactable = false;
        _gemAnimCo = StartCoroutine(CoGemMoveAnim());
    }
    
    private IEnumerator CoGemMoveAnim()
    {
        if (_uiCanvas == null || _gemUI == null)
        {
            yield break;
        }

        _uiCanvas.SpwanIcon(CInstancePanel.EIconType.GemToUI, _endButton.transform.position, 51);
        CPlayerDataManager.Instance.Gem = 510;

        yield return new WaitForSeconds(_waitTime);

        _gemUI.GemTextUpdate();

        yield return new WaitForSeconds(_endWaitTime);

        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.LoadScene(ESceneLoadType.TestLobby);
        }
    }
}