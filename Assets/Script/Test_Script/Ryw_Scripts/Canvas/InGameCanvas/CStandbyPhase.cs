

#region CStandbyPhase
/*

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
        igc.StageGoal.gameObject.SetActive(false);
        igc.StagePanel.gameObject.SetActive(true);
    }

    public void Update(CInGameCanvas igc)
    {
    }

    public void Exit(CInGameCanvas igc)
    {
    }
}
