using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace OM
{
    public class GameStateView : MonoBehaviour
    {
        [HideInInspector] public bool isPauseButtonPressed;
        public GameObject pauseMenuUI;
        public Button ResumeGameButton;
        
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
    }
}
