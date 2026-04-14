

#region CMainPhase
/*
▶ 작성자 류연우

stageGoal 판넬을 보여줌.
*/
#endregion

public class CMainPhase : IInGameCanvasPhaseFSM
{
    #region 내부 변수

    #endregion

    #region 프로퍼티

    #endregion
    public void Enter(CInGameCanvas igc)
    {
        igc.StagePanel.gameObject.SetActive(false);
        igc.StageGoal.gameObject.SetActive(true);
    }

    public void Exit(CInGameCanvas igc)
    {
    }

    public void Update(CInGameCanvas igc)
    {
    }
}
