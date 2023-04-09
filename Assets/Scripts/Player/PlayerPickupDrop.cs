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

        private ObjectGrabbable objectGrabbable;
        private bool isHolding;

        public delegate void PlayerPickUpAction(ItemSO itemSO);
        public static event PlayerPickUpAction OnItemPickUp;

        /// <summary>
        /// Function for picking up items to inventory
        /// </summary>
        public void HandlePickup(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled && !isHolding)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out ItemSOHolder itemSOHolder))
                    {
                        if (itemSOHolder != null)
                        {
                            OnItemPickUp(itemSOHolder.ItemSO);
                            Destroy(raycastHit.transform.gameObject);
                        }
                    }
                }
            }
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