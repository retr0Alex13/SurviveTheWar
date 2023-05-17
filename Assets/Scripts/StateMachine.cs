using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    [Serializable]
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public ResumedState resumedState;
        public PauseState pauseState;

        public StateMachine(GameStateView gameStateView)
        {
            resumedState = new ResumedState(gameStateView);
            pauseState = new PauseState(gameStateView);
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
