internal class OnStateChanged : IGameEvent
{
    private GameEvents newState;
    private GameEvents oldState;
    public GameEvents NewState { get { return newState; } }
    public GameEvents OldState { get { return oldState; } }

    public OnStateChanged(GameEvents newState, GameEvents oldState)
    {
        this.newState = newState;
        this.oldState = oldState;
    }
}