using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class GameOverState : IState
    {
        private bool mainMenuButoon;
        private bool restartButton;
        private GameStateView _gameStateView;

        public GameOverState(GameStateView gameStateView)
        {
            _gameStateView = gameStateView;
        }
        public void Enter()
        {
            _gameStateView.restartButton.onClick.AddListener(delegate
            {
                restartButton = true;
            });
            _gameStateView.mainMenuButton.onClick.AddListener(delegate
            {
                mainMenuButoon = true;
            });
            _gameStateView.deathScreenView.SetVisibleDeathGroupUI();
            _gameStateView.StateMachine.pauseState.OnPause();
        }

        public void Update()
        {
            if (mainMenuButoon)
            {
                _gameStateView.LoadMainMenuScene();
                TransitionToResumedState();
            }
            else if (restartButton)
            {
                _gameStateView.RestartScene();
                TransitionToResumedState();
            }
        }

        public void Exit()
        {
            _gameStateView.StateMachine.pauseState.OnUnpause();
        }

        public void TransitionToResumedState()
        {
            _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.resumedState);
        }
    }
}
