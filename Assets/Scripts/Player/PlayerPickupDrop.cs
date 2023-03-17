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

        private Inventory playerInventory;
        private ObjectGrabbable objectGrabbable;
        private bool isHolding;

        private void Start()
        {
            playerInventory = GetComponent<Inventory>();
        }

        public void HandlePickup(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled && !isHolding)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit2, pickUpDistance))
                {
                    if (raycastHit2.transform.TryGetComponent(out ItemSOHolder itemSOHolder))
                    {
                        if (itemSOHolder != null)
                        {
                            playerInventory.AddItem(itemSOHolder.ItemSO);
                            Destroy(raycastHit2.transform.gameObject);
                        }
                    }
                }
            }
        }

        public void HandleGrab(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (objectGrabbable == null)
                {
                    if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
                    {
                        if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                        {
                            objectGrabbable.Grab(objectGrabPointTransform);
                            isHolding = true;
                        }
                    }
                }
                else
                {
                    Drop();
                }
            }
            else if (objectGrabbable != null)
            {
                Drop();
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