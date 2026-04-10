using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClothes_Unlocker : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private List<GameObject> _clothesSlots;
    [SerializeField] private GameObject _rewardPopup;

    [SerializeField] private float startDelay = 0.05f;
    [SerializeField] private float endDelay = 0.3f;
    #endregion


    private bool isRolling = false;

    public void StartRandomUnlock()
    {
        if (isRolling) return;
        StartCoroutine(CoRandomUnlock());
    }

    IEnumerator CoRandomUnlock()
    {
        isRolling = true;

        List<int> lockedIndex = new List<int>();

        for (int i = 0; i < _clothesSlots.Count; i++)
        {
            GameObject slot = _clothesSlots[i];
            if (slot == null) continue;

            Transform lockObj = slot.transform.Find("Lock");

            if (lockObj != null && lockObj.gameObject.activeSelf)
                lockedIndex.Add(i);
        }

        if (lockedIndex.Count == 0)
        {
            isRolling = false;
            yield break;
        }

        int targetIndex = lockedIndex[Random.Range(0, lockedIndex.Count)];

        // 마지막 1개
        if (lockedIndex.Count == 1)
        {
            int lastIndex = lockedIndex[0];

            SetHighlight(lastIndex);
            yield return new WaitForSeconds(0.1f);

            GameObject selectedSlot = _clothesSlots[lastIndex];

            SetUnlock(selectedSlot);
            SelectByController(selectedSlot);

            ShowReward();

            SetAllHighlightOff();
            isRolling = false;
            yield break;
        }

        int loopCount = Random.Range(6, 10);

        for (int i = 0; i < loopCount; i++)
        {
            int randomPos = Random.Range(0, lockedIndex.Count);
            int slotIndex = lockedIndex[randomPos];

            SetHighlight(slotIndex);

            float t = (float)i / loopCount;
            t = t * t;

            float delay = Mathf.Lerp(startDelay, endDelay, t);
            yield return new WaitForSeconds(delay);
        }

        SetHighlight(targetIndex);
        yield return new WaitForSeconds(endDelay * 1.5f);

        GameObject finalSlot = _clothesSlots[targetIndex];

        SetUnlock(finalSlot);
        SelectByController(finalSlot);

        ShowReward();

        SetAllHighlightOff();
        isRolling = false;
    }

    // Controller에게 선택 맡김
    private void SelectByController(GameObject slot)
    {
        var controller = FindObjectOfType<ClothesSelect_Controller>();
        if (controller != null)
        {
            controller.SelectSlot(slot);
        }
    }

    private void ShowReward()
    {
        if (_rewardPopup != null)
            _rewardPopup.SetActive(true);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SFXPlay(ESfxType.Item_Unlock);
    }

    // Unlock은 열기만
    private void SetUnlock(GameObject slot)
    {
        Transform lockObj = slot.transform.Find("Lock");
        Transform openObj = slot.transform.Find("Open");

        if (lockObj != null) lockObj.gameObject.SetActive(false);
        if (openObj != null) openObj.gameObject.SetActive(true);
    }

    void SetHighlight(int index)
    {
        for (int i = 0; i < _clothesSlots.Count; i++)
        {
            Transform highlight = _clothesSlots[i].transform.Find("Highlight");

            if (highlight != null)
            {
                bool isOn = (i == index);

                if (isOn && !highlight.gameObject.activeSelf)
                {
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.SFXPlay(ESfxType.Roullet);
                }

                highlight.gameObject.SetActive(isOn);
            }
        }
    }

    void SetAllHighlightOff()
    {
        for (int i = 0; i < _clothesSlots.Count; i++)
        {
            Transform highlight = _clothesSlots[i].transform.Find("Highlight");

            if (highlight != null)
                highlight.gameObject.SetActive(false);
        }
    }
}
