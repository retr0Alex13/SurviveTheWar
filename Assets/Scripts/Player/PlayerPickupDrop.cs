using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

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

        private void Start()
        {
            playerInventory = GetComponent<Inventory>();
        }

        public void HandlePickup(InputAction.CallbackContext ctx)
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
            //if (ctx.canceled)
            //{
            //    if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit2, pickUpDistance))
            //    {
            //        if (raycastHit2.transform.TryGetComponent(out ItemSOHolder itemSOHolder))
            //        {
            //            if (itemSOHolder != null)
            //            {
            //                playerInventory.AddItem(itemSOHolder.ItemSO);
            //                Destroy(raycastHit2.transform.gameObject);
            //            }
            //        }
            //    }
            //}
        }

        private void Drop()
        {
            objectGrabbable.Drop();
            objectGrabbable = null;
        }
    }
}