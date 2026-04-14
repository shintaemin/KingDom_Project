

#region CStandbyPhase
/*
스테이지 판넬을 보여줌.
스테이지 골 판넬을 보여줌.
*/
#endregion

public class CStandbyPhase : IInGameCanvasPhaseFSM
{
    #region 내부 변수

    #endregion

    #region 프로퍼티

    #endregion
    public void Enter(CInGameCanvas igc)
    {
        igc.StageGoal.gameObject.SetActive(true);
        //if(igc.) 서브스테이지가 1인 경우. == 첫방인 경우에만 활성화한다.
        igc.StagePanel.gameObject.SetActive(true);
    }

    public void Update(CInGameCanvas igc)
    {
    }

    public void Exit(CInGameCanvas igc)
    {
    }
}
