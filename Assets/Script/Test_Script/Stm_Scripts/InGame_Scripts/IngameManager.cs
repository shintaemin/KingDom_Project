using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 인게임 매니저
/*
 ▶ 할일
  - 플레이어 레벨(stage)데이터 확인하고 해당되는 맵 배치
  - 미션매니저의 SetMiision(맵); 함수 호출하여 맵 던져주기
  - 스포너에게 생성할 맵과 데이터 던져주기
*/
#endregion


public class IngameManager : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private MissionManager _msManager;
    [SerializeField] private Map_Registry_SO _mapSO;
    [SerializeField] private Map_Stage _currentMap;
    [SerializeField] private SpawnManager _sm;
    //[SerializeField] private Player_Data_SO _playerData;
    [SerializeField] private int _mapIndex = 1;
    #endregion

    #region 외부 호출 함수

    #endregion

    private void Awake()
    {
        
    }

    #region 미션 클리어 구독
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

    // 미션 클리어시 호출함수
    private void AddIndex()
    {
        // 다음 맵으로 이동 하기위해 인덱스 변경
        _mapIndex++;
        // 미션 클리어하여 해당 미션 구독 취소
        MissionClear();

        // 데이터 다시확인하고 맵 재지정
        //int stateData = _playerData.GetStageData;

        SetMap(1/*stateData*/, _mapIndex);
    }
    #endregion


    // 게임 시작시 맵 데이터값 받아와서 맵지정
    private void Start()
    {
        //int stateData = _playerData.GetStageData;

        _mapIndex = 1;
        SetMap(1/*stateData*/, _mapIndex);
    }

    private void SetMap(int stageData, int mapIndex)
    {
        if (_msManager == null || _mapSO == null || _sm == null)
        {
            return;
        }

        Map_Stage map = _mapSO.GetMap(stageData, mapIndex);

        // 맵이 없다면
        if (map == null)
        {

            // 여기서 씬종료로 연결
            _currentMap = null;
            return;
        }

        // 같은 맵이 아닐떄 맵 할당
        if (_currentMap != map)
        {
            _currentMap = map;
        }

        // 미션에 맵전달
        _msManager.SetMission(_currentMap);
        // 스포너에 맵전달
        _sm.SetMap(map);
        // 구독 진행
        Subscription();
    }
}