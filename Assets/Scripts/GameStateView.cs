using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace OM
{
    public class GameStateView : MonoBehaviour
    {
        [HideInInspector] public bool isPauseButtonPressed;
        public DeathScreenView deathScreenView;
        public GameObject pauseMenuUI;
        public Button resumeGameButton;
        public Button restartButton;
        public Button toMainMenButton;
        
        private StateMachine stateMachine;
        public StateMachine StateMachine => stateMachine;

        private void Awake()
        {
            stateMachine = new StateMachine(this);
        }

        private void Start()
        {
            stateMachine.Initialize(stateMachine.resumedState);
        }

        private void Update()
        {
            stateMachine.Update();
        }

        public void IsPauseButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isPauseButtonPressed = !isPauseButtonPressed;
            }
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
