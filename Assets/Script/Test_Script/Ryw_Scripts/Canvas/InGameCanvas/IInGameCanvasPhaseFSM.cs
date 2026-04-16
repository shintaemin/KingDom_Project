

#region IGoalState
/*
▶ 작성자 류연우

인게임 캔버스를 위한 상태머신 인터페이스

이건 제네릭이 안되나?
이걸 사용할 클래스를 위한 인터페이스나 클래스도 있으면 좋을 듯.
*/
#endregion

public interface IInGameCanvasPhaseFSM
{
    public void Enter(CInGameCanvas igc);
    public void Update(CInGameCanvas igc);
    public void Exit(CInGameCanvas igc);
}
