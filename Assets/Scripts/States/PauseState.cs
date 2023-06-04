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

        private bool isExitButtonPressed;

        private GameStateView _gameStateView;

        public PauseState(GameStateView gameStateView)
        {
            _gameStateView = gameStateView;
        }
        
        public void Enter()
        {
            OnGamePaused?.Invoke(true);
            
            _gameStateView.resumeGameButton.onClick.AddListener(delegate 
            {
                _gameStateView.isPauseButtonPressed = false;
            });
            
            _gameStateView.exitButton.onClick.AddListener(delegate
            {
                isExitButtonPressed = true;
            });
            
            _gameStateView.pauseMenuUI.SetActive(true);
            OnPause();
        }

        public void Update()
        {
            if (!_gameStateView.isPauseButtonPressed)
            {
                _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.resumedState);
            }
            else if (isExitButtonPressed)
            {
                _gameStateView.StateMachine.TransitionTo(_gameStateView.StateMachine.resumedState);
                _gameStateView.LoadMainMenuScene();
            }
        }

        public void Exit()
        {
            OnGamePaused?.Invoke(false);
            
            _gameStateView.optionsMenu.SetActive(false);
            
            _gameStateView.pauseMenuUI.SetActive(false);

            _gameStateView.isPauseButtonPressed = false;

            isExitButtonPressed = false;
            
            OnUnpause();
        }

        public void OnPause()
        {
            Cursor.lockState = CursorLockMode.None;
            
            Time.timeScale = 0f;

            AudioListener.pause = true;

        }
        
        public void OnUnpause()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            Time.timeScale = 1f;
            
            AudioListener.pause = false;

        }
    }
}
