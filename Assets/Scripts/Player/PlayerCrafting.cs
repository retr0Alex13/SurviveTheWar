using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerCrafting : MonoBehaviour
    {
        [SerializeField] private float interactDistance = 2f;
        [SerializeField] private LayerMask interactLayerMask;
        [SerializeField] private Transform playerCameraTransform;

        private void Start()
        {

        }

        public void HandleCrafting(InputAction.CallbackContext ctx)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, interactDistance, interactLayerMask) 
                && raycastHit.transform.TryGetComponent(out CraftingStation craftingWorkbench))
            {
                if (ctx.performed)
                {
                    craftingWorkbench.Craft();
                }
                if (ctx.canceled)
                {
                    craftingWorkbench.NextRecipe();
                }
            }
        }
    }
}