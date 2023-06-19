using System;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerPickupDrop : MonoBehaviour
    {
        [SerializeField] private float pickUpDistance = 2f;
        [SerializeField] private Transform playerCameraTransform;
        [SerializeField] private Transform objectGrabPointTransform;
        private PlayerEquipSlot playerEquipSlot;
        private InventoryMediator inventory;

        private ObjectGrabbable objectGrabbable;
        private bool isHolding;

        public delegate void PlayerPickUpAction(ItemSO itemSO, ItemSOHolder itemSOHolder);
        public static event PlayerPickUpAction OnItemPickUp;

        private void Awake()
        {
            inventory = GetComponent<InventoryMediator>();
            playerEquipSlot = GetComponent<PlayerEquipSlot>();
        }

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
                            if (inventory.IsInventoryFull(itemSOHolder.ItemSO))
                                return;
                            
                            else if (itemSOHolder.ItemSO.itemType == ItemSO.ItemType.Gasoline)
                            {
                                ProceedToEquipItem(itemSOHolder, raycastHit);
                                return;
                            }
                            else if (itemSOHolder.ItemSO.itemType == ItemSO.ItemType.Equipable)
                            {
                                ProceedToEquipItem(itemSOHolder, raycastHit);
                                return;
                            }

                            OnItemPickUp(itemSOHolder.ItemSO, itemSOHolder);

                            Destroy(raycastHit.transform.gameObject);
                        }
                    }
                }
            }
        }

        private void ProceedToEquipItem(ItemSOHolder itemSOHolder, RaycastHit raycastHit)
        {
            if (playerEquipSlot.currentlyequipedItem != null)
                return;

            Debug.Log("Equipping item..");

            if (itemSOHolder.gameObject.TryGetComponent(out ItemDurability itemDurability))
            {
                Debug.Log(itemDurability.ItemSO + " durability when proceeded to equip " + itemDurability.CurrentDurability);
                playerEquipSlot.EquipItem(itemSOHolder, itemDurability);
            }
            else
            {
                playerEquipSlot.EquipItem(itemSOHolder, null);
            }
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
                    objectGrabbable.Drop();
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
                objectGrabbable.Drop();
                Drop();
            }
        }

        private void TryGrab()
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance) 
                && raycastHit.transform.TryGetComponent(out objectGrabbable))
            {
                objectGrabbable.Grab(objectGrabPointTransform);
                isHolding = true;
            }
        }

        private void Drop()
        {
            objectGrabbable = null;
            isHolding = false;
        }
    }
}