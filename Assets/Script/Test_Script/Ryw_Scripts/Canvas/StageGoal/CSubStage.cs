using UnityEngine;
using UnityEngine.UI;


#region CSubStagePanel
/*
▶ 작성자 류연우
*/
#endregion

public class CSubStage : MonoBehaviour
{
    #region 인스펙터
    [Header("확인용. 값을 넣는다고 딱히 뭔가 일어나지 않음.")]
    [SerializeField] private int _maxSubStage;
    [SerializeField] private int _currentSubStage;

    [Header("스프라이트")]
    [SerializeField] private Sprite _clear;
    [SerializeField] private Sprite _non;
    [SerializeField] private Sprite _now;
    [Header("프리팹")]
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private GameObject _barPrefab;
    #endregion

    #region 내부 변수
    private CBubble[] _bubbles;
    private GameObject[] _bars;
    #endregion

    public int MaxSubStage { get { return _maxSubStage; } set { SetMaxSubStage(value); } }
    public int CurrentSubStage { get { return _currentSubStage; } set { SetCurrentSubStage(value); } }

    public void Awake()
    {
        if (_clear.IsNull("_clear") ||
            _non.IsNull("_non") ||
            _now.IsNull("_now") ||
            _bubblePrefab.IsNull("_bubblePrefab") ||
            _barPrefab.IsNull("_barPrefab"))
        {
            return;
        }
    }

    private void SetMaxSubStage(int maxSubStage)
    {
        if (_bubbles != null)
        {
            _bubbles = null;
        }
        if (_bars != null)
        {
            _bars = null;
        }

        _maxSubStage = maxSubStage;

        if (MaxSubStage == 1)
        {
            return;
        }
        else if (MaxSubStage < 1)
        {
            Debug.LogWarning($"이럴리가 없다. 뭔가 이상하다. => {MaxSubStage < 1}");
        }
        // 버블 만든다.
        _bubbles = new CBubble[MaxSubStage];
        _bars = new GameObject[MaxSubStage - 1];
        float length = 0;

        CBubble bubble;

        GameObject go = Instantiate(_bubblePrefab, transform);
        if (go.IsNull("gameObject"))
        {
            return;
        }
        go.transform.localPosition = Vector3.zero;


        if (go.TryGetComponent(out bubble))
        {
            _bubbles[0] = bubble;
            bubble.SetImage(_now);
            bubble.SetText("1", Color.white);

            length += bubble.Image.rectTransform.rect.width;
        }


        for (int i = 1; i < _maxSubStage; i++)
        {
            // 바를 만든다.
            go = Instantiate(_barPrefab, transform);
            if (go.IsNull("gameObject"))
            {
                return;
            }

            Vector3 pos = Vector3.zero;

            _bars[i - 1] = go;

            if (go.TryGetComponent(out Image image))
            {
                float width = image.rectTransform.rect.width;

                pos.x += length - width / 2;
                go.transform.localPosition = pos;

                length += width;
            }

            // 버블 만든다.
            go = Instantiate(_bubblePrefab, transform);
            if (go.IsNull("gameObject"))
            {
                return;
            }

            pos = Vector3.zero;
            pos.x += length;
            go.transform.localPosition = pos;


            if (go.TryGetComponent(out bubble))
            {
                _bubbles[i] = bubble;
                bubble.SetImage(_non);
                bubble.SetText($"{i + 1}", Color.white);

                length += bubble.Image.rectTransform.rect.width;
            }
        }
        // 최종 위치를 조정한다.
        length -= 60;   // 버블 하나의 크기
        length /= 2;
        for (int i = 0; i < _bars.Length; i++)
        {
            Vector3 pos = _bars[i].transform.position;
            pos.x -= length;
            _bars[i].transform.position = pos;
        }

        for (int i = 0; i < _bubbles.Length; i++)
        {
            Vector3 pos = _bubbles[i].transform.position;
            pos.x -= length;
            _bubbles[i].transform.position = pos;
        }
    }

    private void SetCurrentSubStage(int currentSubStage)
    {
        _currentSubStage = currentSubStage;

        if (MaxSubStage == 0)
        {
            Debug.LogWarning($"무언가 잘못되었다. {MaxSubStage == 0}");
        }
        else if (MaxSubStage == 1)
        {
            return;
        }
        else
        {
            Debug.LogWarning($"[CSubStage] : 최대 스테이지 갯수 {MaxSubStage} , 선택스테이지 {currentSubStage}");

            for (int i = 0; i < _bubbles.Length; i++)
            {
                // 이것도 이전 라운드부터 검사해도 문제 없다.
                // _currentSubStage는 1부터 시작하지만 배열의 주소는 0부터 시작한다.
                if (i < _currentSubStage - 1)
                {
                    _bubbles[i].SetImage(_clear);
                    _bubbles[i].SetText("V", Color.green);
                }
                else if (i == _currentSubStage - 1)
                {
                    _bubbles[i].SetImage(_now);
                    _bubbles[i].SetText($"{i + 1}", Color.white);
                }
                // 이론상 여기는 없어도 된다.
                else
                {
                    _bubbles[i].SetImage(_non);
                    _bubbles[i].SetText($"{i + 1}", Color.white);
                }
            }
        }
    }
}
