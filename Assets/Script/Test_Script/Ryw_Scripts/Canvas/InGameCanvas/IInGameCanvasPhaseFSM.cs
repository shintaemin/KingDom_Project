

#region IGoalState
/*
▶ 작성자 류연우

인게임 캔버스를 위한 상태머신 인터페이스
*/
#endregion

public interface IInGameCanvasPhaseFSM
{
    public void Enter(CInGameCanvas igc);
    public void Update(CInGameCanvas igc);
    public void Exit(CInGameCanvas igc);
}
