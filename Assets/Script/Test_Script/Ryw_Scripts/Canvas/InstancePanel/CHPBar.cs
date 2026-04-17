using TMPro;
using UnityEngine;
using UnityEngine.UI;


#region CHPBar
/*
▶ 작성자 류연우

출력 대상은 IHPBar를 상속받고 멤버를 구현해야 한다.

체력의 변동이 있을때만 잠깐 보이는 기능도 필요하다.
알파값을 변경하는 애니메이션을 트리거로 작동시킨다.
*/
#endregion

public class CHPBar : MonoBehaviour
{
    #region 인스펙터
    [Header("일정 시간 표시 관련")]
    [SerializeField] private bool _showOnlyOnChanged = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramAnimation = "Animation";
    [SerializeField] private Image _fillImage;

    [Header("그 외")]
    [SerializeField] private GameObject _hpSlierPrefab;
    [SerializeField] private Transform _spawnRoot;
    [Header("mesh 마다, 화면 사이즈마다 다를 것")]// 그냥 position을 사용하면 안된다. UI 전용으로 해야하는데 지금화서 하기에는 너무 늦었고
    [SerializeField] private Vector3 _UIOffset = new Vector3(0, 50f,0);
    #endregion

    #region 내부 변수
    private int _hashAnimation;
    private Slider _hpSlider;
    private IHPBar _target;
    #endregion

    void Awake()
    {
        if (_hpSlierPrefab.IsNull("_hpSlierPrefab") ||
            _spawnRoot.IsNull("_spawnRoot"))
        {
            Debug.LogWarning("캔버스의 인스턴스 패널의 스폰 루트를 추가");
        }

        _target = GetComponent<IHPBar>();
        if(_target.IsNull("IHPBar"))
        {
            Debug.LogWarning("이 컴포넌트를 사용하는 오브젝트는 IHPBar를 상속받는 컴포넌트가 포함되어있어야 한다.");
            return;
        }

        _hashAnimation = Animator.StringToHash(_paramAnimation);
    }

    public void InitSpawnPos(Transform tr)
    {
        if (_spawnRoot != null)
        {
            Debug.Log($"[CHPBar] : 스폰위치 이미 지정됨");
            _spawnRoot = null;
        }

        _spawnRoot = tr;

        GameObject go = Instantiate(_hpSlierPrefab, _spawnRoot);
        if (go.IsNull("gameObject"))
        {
            return;
        }
        else
        {
            Debug.Log("인스턴스 생성?");
        }
        if (go.TryGetComponent(out Slider prefab))
        {
            _hpSlider = prefab;
        }
        if (go.TryGetComponent<Animator>(out Animator animator))
        {
            _animator = animator;
        }

        _fillImage = go.transform.Find("Fill Area/Fill").GetComponent<Image>();
        if (_fillImage == null)
        {
            Debug.Log($"[CHPBar] : _fillImage 캐싱 실패");
        }
        else
        {
            Debug.Log($"[CHPBar] : _fillImage 캐싱 성공");
        }
    }

    public void SetFillColor(Color col)
    {
        if (_hpSlierPrefab == null)
        {
            Debug.Log($"[CHPBar] : _hpSlierPrefab 이 없음");
            return;
        }
        if (_fillImage == null)
        {
            Debug.Log($"[CHPBar] : _fillImage 이 없음");
            return;
        }

        Debug.Log($"[CHPBar] : _fillImage 색상 적용 성공");
        _fillImage.color = col;
    }

    private void OnEnable()
    {
        if (_target != null)
        {
            _target.OnHealthChanged += UpdateSlider;
            _target.OnPositionChanged += UpdatePosition;
            _target.OnDead += OnDead;
        }
    }

    private void OnDisable()
    {
        if (_target != null)
        {
            _target.OnHealthChanged -= UpdateSlider;
            _target.OnPositionChanged -= UpdatePosition;
            _target.OnDead -= OnDead;
        }
    }

    private void OnDead()
    {
        _target.OnHealthChanged -= UpdateSlider;
        _target.OnPositionChanged -= UpdatePosition;
        _target.OnDead -= OnDead;
        Object.Destroy(_hpSlider.gameObject);
    }

    private void UpdateSlider(float ratio)
    {
        if (_hpSlider == null)
        {
            return;
        }

        _hpSlider.value = ratio;
        if (_showOnlyOnChanged)
        {
            // 슬라이더 알파값을조절하는 코루틴 함수 호출.
            _animator.SetTrigger(_hashAnimation);
        }
    }

    private void UpdatePosition(Vector3 position)
    {
        if (_hpSlider == null)
        {
            return;
        }

        if (CInGameCanvas.WorldToUI(position, out Vector3 UIPos))
        {
            Vector3 pos = UIPos;
            pos += _UIOffset;
            _hpSlider.transform.position = pos;
        }
    }
}
