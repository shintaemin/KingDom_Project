using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 아이템 슬롯 랜덤 해금 관리
/*
 ▶ 할일
  - 잠겨있는 아이템 중 하나를 랜덤으로 선택하여 해금
  - Lock → Open 상태로 변경
  - 모든 아이템이 해금된 경우 동작하지 않음

 ▶ 흐름
  1. 모든 슬롯 검사
  2. 잠긴 슬롯만 리스트에 수집
  3. 랜덤으로 하나 선택
  4. 선택된 슬롯을 해금 상태로 변경

- 박라희
*/
#endregion

public class RandomWeapon_Unlocker : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weaponSlots;
    [SerializeField] private GameObject _rewardPopup;

    [SerializeField] private float startDelay = 0.05f;
    [SerializeField] private float endDelay = 0.3f;

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

        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            GameObject slot = _weaponSlots[i];
            if (slot == null) continue;

            Transform lockObj = slot.transform.Find("Lock");

            if (lockObj != null && lockObj.gameObject.activeSelf)
            {
                lockedIndex.Add(i);
            }
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

            SetUnlock(_weaponSlots[lastIndex]);

            if (_rewardPopup != null)
                _rewardPopup.SetActive(true);

            if (SoundManager.Instance != null)
                SoundManager.Instance.SFXPlay(ESfxType.Item_Unlock);

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

        SetUnlock(_weaponSlots[targetIndex]);

        if (_rewardPopup != null)
            _rewardPopup.SetActive(true);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SFXPlay(ESfxType.Item_Unlock);

        SetAllHighlightOff();

        isRolling = false;
    }

    void SetHighlight(int index)
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            Transform highlight = _weaponSlots[i].transform.Find("Highlight");

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
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            Transform highlight = _weaponSlots[i].transform.Find("Highlight");

            if (highlight != null)
                highlight.gameObject.SetActive(false);
        }
    }

    private void SetUnlock(GameObject slot)
    {
        Transform lockObj = slot.transform.Find("Lock");
        Transform openObj = slot.transform.Find("Open");
        Transform checkObj = slot.transform.Find("Check");

        if (lockObj != null) lockObj.gameObject.SetActive(false);
        if (openObj != null) openObj.gameObject.SetActive(true);

        SetAllCheckOff();

        if (checkObj != null) checkObj.gameObject.SetActive(true);
    }

    void SetAllCheckOff()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            Transform check = _weaponSlots[i].transform.Find("Check");

            if (check != null)
                check.gameObject.SetActive(false);
        }
    }
}
