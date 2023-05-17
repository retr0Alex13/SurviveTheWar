using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class ResumedState : IState
    {
        private GameStateView _gameStateView;

        public ResumedState(GameStateView gameStateView)
        {
            this._gameStateView = gameStateView;
        }
        public void Enter()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            if (_gameStateView.isPauseButtonPressed)
            {
                _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.pauseState);
            }
        }

        public void Exit()
        {
            
        }
    }
}
