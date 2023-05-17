using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class PauseState : IState
    {
        public delegate void PausedStateAction(bool isPaused);
        public static event PausedStateAction OnGamePaused;
        
        private GameStateView _gameStateView;

        public PauseState(GameStateView gameStateView)
        {
            this._gameStateView = gameStateView;
        }
        
        public void Enter()
        {
            OnGamePaused?.Invoke(true);
            _gameStateView.ResumeGameButton.onClick.AddListener(delegate 
            {
                _gameStateView.isPauseButtonPressed = false;
            });
            OnPause();
        }

        public void Update()
        {
            if (!_gameStateView.isPauseButtonPressed)
            {
                _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.resumedState);
            }
        }

        public void Exit()
        {
            OnGamePaused?.Invoke(false);
            OnUnpause();
        }

        private void OnPause()
        {
            Cursor.lockState = CursorLockMode.None;
            
            _gameStateView.pauseMenuUI.SetActive(true);

            Time.timeScale = 0f;

            AudioListener.pause = true;

        }
        
        private void OnUnpause()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            _gameStateView.pauseMenuUI.SetActive(false);

            Time.timeScale = 1f;
            
            AudioListener.pause = false;

        }
    }
}
