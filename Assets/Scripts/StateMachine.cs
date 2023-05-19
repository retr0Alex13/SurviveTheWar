using System;


namespace OM
{
    [Serializable]
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public ResumedState resumedState;
        public PauseState pauseState;
        public GameOverState gameOverState;

        public StateMachine(GameStateView gameStateView)
        {
            resumedState = new ResumedState(gameStateView);
            pauseState = new PauseState(gameStateView);
            gameOverState = new GameOverState(gameStateView);
        }

        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }
        
        
    }
}
