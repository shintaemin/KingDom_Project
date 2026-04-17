using System;
using UnityEngine;


#region EnergyTimer
/*
▶ 작성자 류연우
*/
#endregion

public class EnergyTimer : MonoBehaviour
{
    private static readonly string TIMEKEY = "LastTime";
    #region 내부 변수
    public event Action<double> _elapsedTime;
    private DateTime _lastTime;
    #endregion

    void Awake()
    {
        // 이게 살아날때 시간을 불러온다.
        string lastTime = PlayerPrefs.GetString(TIMEKEY);
        if (DateTime.TryParse(lastTime, out _lastTime)) { }
        else
        {
            _lastTime = new DateTime();
        }

        if (CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.Energetimer = this;
            CPlayerDataManager.Instance.EnergeTimerSub();
        }
    }

    private void Start()
    {
        Timeelapsed();
    }
    private void OnDestroy()
    {
        // 여기서 호출하므로 유니티 라이프 사이클에 의해
        // 다른 구독자들이 메시지를 받지 못한다.
        // == 시간을 저장만 한다.
        Timeelapsed();

        if (CPlayerDataManager.Instance != null)
        {
            CPlayerDataManager.Instance.EnergeTimerDis();
        }
    }

    public void Timeelapsed()
    {
        DateTime now = DateTime.UtcNow;
        if (_lastTime != now)
        {
            PlayerPrefs.SetString(TIMEKEY, DateTime.UtcNow.ToString());

            // 경과시간
            TimeSpan elapsed = now - _lastTime;
            Debug.Log($"elapsed:{elapsed} = now:{now} - _lastTime:{_lastTime}");
            _elapsedTime?.Invoke(elapsed.TotalSeconds);

            // 마지막 시간 업데이트
            _lastTime = now;
        }
    }
}
