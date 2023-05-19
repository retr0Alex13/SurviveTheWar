using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class ResumedState : IState
    {
        private bool isPlayerDead;
        private GameStateView _gameStateView;
        public ResumedState(GameStateView gameStateView)
        {
            _gameStateView = gameStateView;
        }
        public void Enter()
        {
            PlayerHealth.OnPlayerDead += SetPlayerDead;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            if (_gameStateView.isPauseButtonPressed)
            {
                _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.pauseState);
            }

            if (isPlayerDead)
            {
                _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.gameOverState);
            }
        }

        public void Exit()
        {
            PlayerHealth.OnPlayerDead -= SetPlayerDead;
            isPlayerDead = false;
        }

        private void SetPlayerDead()
        {
            isPlayerDead = true;
        }
    }
}
