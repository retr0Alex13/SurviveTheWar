using System;
using UnityEngine;
using OM;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        [SerializeField] private Vector2 move;
        [SerializeField] private Vector2 look;
        [SerializeField] private bool jump;
        [SerializeField] private bool sprint;
        [SerializeField] private bool pickup;
        [SerializeField] private bool interact;
        [SerializeField] private bool openInventory;
        [SerializeField] private bool escapeKey;

        [Header("Movement Settings")]
        [SerializeField] private bool analogMovement;

        [Header("Mouse Cursor Settings")]
        [SerializeField] private bool cursorLocked = true;
        [SerializeField] private bool cursorInputForLook = true;

        void OnEnable()
        {
            PlayerNeeds.OnExhausted += SprintInput;
        }

        void OnDisable()
        {
            PlayerNeeds.OnExhausted -= SprintInput;
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (cursorInputForLook)
            {
                LookInput(context.ReadValue<Vector2>());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpInput(context.ReadValueAsButton());
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            SprintInput(context.action.ReadValue<float>() == 1);
        }

        public void OnPickup(InputAction.CallbackContext context)
        {
            PickupInput(context.performed);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {

            InteractInput(context.performed);
        }

        public void OnInventoryOpen(InputAction.CallbackContext context)
        {
            InventoryOpenInput(context.performed);
            SetCursorState(!context.performed);
        }

        public void OnEscapeKeyDown(InputAction.CallbackContext context)
        {
            EscapeKeyInput(context.performed);
        }

#endif
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void PickupInput(bool newPickupState)
        {
            pickup = newPickupState;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void InventoryOpenInput(bool newInventoryState)
        {
            openInventory = newInventoryState;
        }

        private void EscapeKeyInput(bool newEscapeKeyState)
        {
            escapeKey = newEscapeKeyState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public Vector2 GetMove()
        {
            return move;
        }

        public Vector2 GetLook()
        {
            return look;
        }

        public bool IsJumping()
        {
            return jump;
        }

        public bool IsSprinting()
        {
            return sprint;
        }

        public bool IsAnalog()
        {
            return analogMovement;
        }

        public bool IsPickingup()
        {
            return pickup;
        }

        public bool IsInteracting()
        {
            return interact;
        }

        public bool IsOpeningInventory()
        {
            return openInventory;
        }

        public bool IsEscapeButtonDown()
        {
            return escapeKey;
        }
    }

}