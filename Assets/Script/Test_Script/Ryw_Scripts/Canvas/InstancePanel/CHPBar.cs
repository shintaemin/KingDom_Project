using System.Collections;
using Unity.VisualScripting;
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
    [Header("일도양단")]
    [SerializeField] private bool _showOnlyOnChanged = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramAnimation = "Animation";

    [Header("그 외")]

    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Transform _spawnRoot;
    [SerializeField] private IHPBar _target;
    #endregion

    #region 내부 변수
    private int _hashAnimation;
    #endregion

    void Awake()
    {
        if (_hpSlider.IsNull("_hpSlider") ||
            _spawnRoot.IsNull("_spawnRoot") ||
            _target.IsNull("_target"))
        {
            Debug.LogWarning("캔버스의 인스턴스 패널의 스폰 루트를 추가");
            return;
        }
        _hashAnimation = Animator.StringToHash(_paramAnimation);
    }

    private void OnEnable()
    {
        if (_target != null)
        {
            _target.OnHealthChanged += UpdateSlider;
            _target.OnPositionChanged += UpdatePosition;
        }
    }

    private void OnDisable()
    {
        if (_target != null)
        {
            _target.OnHealthChanged -= UpdateSlider;
            _target.OnPositionChanged -= UpdatePosition;
        }
    }

    private void UpdateSlider(float ratio)
    {
        _hpSlider.value = ratio;
        if (_showOnlyOnChanged)
        {
            // 슬라이더 알파값을조절하는 코루틴 함수 호출.
            _animator.SetTrigger(_hashAnimation);
        }
    }

    private void UpdatePosition(Vector3 position)
    {
        if (CInGameCanvas.WorldToUI(position, out Vector3 UIPos))
        {
            this.transform.position = UIPos;
        }
    }
}
