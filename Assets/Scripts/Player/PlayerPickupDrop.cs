using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerPickupDrop : MonoBehaviour
    {
        [SerializeField] private float pickUpDistance = 2f;
        [SerializeField] private Transform playerCameraTransform;
        [SerializeField] private Transform objectGrabPointTransform;
        [SerializeField] private LayerMask pickUpLayerMask;
        [SerializeField] private InventoryMediator inventoryMediator;

        private ObjectGrabbable objectGrabbable;
        private bool isHolding;

        public delegate void PlayerPickUpAction(ItemSO itemSO);

        public static event PlayerPickUpAction OnItemPickUp;

        /// <summary>
        /// Function for picking up items to inventory
        /// </summary>
        public void HandlePickup(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled || isHolding)
            {
                return;
            }

            if (!Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
            {
                return;
            }

            if (!raycastHit.transform.TryGetComponent<ItemSOHolder>(out var itemSOHolder) || itemSOHolder == null)
            {
                return;
            }

            OnItemPickUp?.Invoke(itemSOHolder.ItemSO);
            Destroy(raycastHit.transform.gameObject);
        }

        /// <summary>
        /// Function for grabbing and holding items
        /// </summary>
        public void HandleGrab(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
            {
                if (objectGrabbable != null)
                {
                    Drop();
                }
                return;
            }

            if (objectGrabbable == null)
            {
                TryGrab();
            }
            else
            {
                Drop();
            }
        }

        private void TryGrab()
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance) && raycastHit.transform.TryGetComponent(out objectGrabbable))
            {
                objectGrabbable.Grab(objectGrabPointTransform);
                isHolding = true;
            }
        }

        private void Drop()
        {
            objectGrabbable.Drop();
            objectGrabbable = null;
            isHolding = false;
        }
    }
}