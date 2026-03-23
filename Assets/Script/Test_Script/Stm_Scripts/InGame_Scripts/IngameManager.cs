using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인게임 매니저
/*
 ▶ 할일
  - 플레이어 레벨(stage)데이터 확인하고 해당되는 맵 배치
  - 미션매니저의 SetMiision(맵); 함수 호출하여 맵 던져주기
  - 스포너에게 생성할 맵 던져주기
*/
#endregion


public class IngameManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private MissionManager _msManager;
    [SerializeField] private Map_Registry_SO _mapSO;
    [SerializeField] private Map_Stage _currentMap;
    //[SerializeField] private Player_Data_SO _playerData;
    [SerializeField] private int _mapIndex = 1;
    #endregion

    #region 외부 호출 함수

    #endregion

    private void Awake()
    {
        
    }

    private void Subscription()
    {
        if (_msManager == null)
        {
            return;
        }

        if (_msManager.GetMission != null)
        {
            _msManager.GetMission.OnClearMission += AddIndex;
            Debug.Log("[IngameManager] : 클리어 미션 구독완료!");
        }
    }

    private void MissionClear()
    {
        if (_msManager != null)
        {
            _msManager.GetMission.OnClearMission -= AddIndex;
            _currentMap = null;
        }
    }

    private void Start()
    {
        //int stateData = _playerData.GetStageData;

        _mapIndex = 1;
        SetMap(1/*stateData*/, _mapIndex);
    }

    private void SetMap(int stageData, int mapIndex)
    {
        if (_msManager == null || _mapSO == null)
        {
            return;
        }

        Map_Stage map = _mapSO.GetMap(stageData, mapIndex);

        if (map == null)
        {

            // 여기서 씬종료로 연결
            return;
        }

        if (_currentMap != map)
        {
            _currentMap = map;
        }

        _msManager.SetMission(_currentMap);
        Subscription();
        // 여기서 스포너를 통해 맵 생성 완료후 적 생성 함수 호출
    }

    private void AddIndex()
    {
        _mapIndex++;
        MissionClear();

        //int stateData = _playerData.GetStageData;

        SetMap(1/*stateData*/, _mapIndex);
    }
}