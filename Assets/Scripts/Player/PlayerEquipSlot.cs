using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerEquipSlot : MonoBehaviour
    {
        [HideInInspector] public Transform currentlyequipedItem;

        [SerializeField] private Transform equipSlot;
        [SerializeField] private Transform dropItemPoint;

        private ItemSO itemSO;
        private ItemSOHolder itemSOHolder;
        private IInteractable interactable;

        private void Awake()
        {
            Gasoline.OnGasolineUsed += RemoveItem;
        }

        private void OnDestroy()
        {
            Gasoline.OnGasolineUsed -= RemoveItem;
        }

        public void EquipItem(ItemSOHolder itemData)
        {
            itemSOHolder = itemData;
            itemSO = itemData.ItemSO;
            foreach(Transform transform in equipSlot)
            {
                ItemSO item = transform.GetComponent<ItemSOHolder>().ItemSO;
                if (item == itemSO)
                {
                    currentlyequipedItem = transform;
                    currentlyequipedItem.gameObject.SetActive(true);
                    interactable = currentlyequipedItem.GetComponent<IInteractable>();
                    interactable.SetDurability(itemData.CurrentDurability);
                }
            }
        }

        public void UnEquipItem(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                if (!CurrentlyEquipedItem())
                    return;

                currentlyequipedItem.gameObject.SetActive(false);
                GameObject dropItem = Instantiate(itemSOHolder.ItemSO.Prefab, new Vector3(dropItemPoint.position.x, dropItemPoint.position.y, dropItemPoint.position.z),
                Quaternion.identity);
                dropItem.GetComponent<ItemSOHolder>().CurrentDurability = currentlyequipedItem.GetComponent<ItemSOHolder>().CurrentDurability;
                Debug.Log(dropItem.GetComponent<ItemSOHolder>().CurrentDurability);
                itemSOHolder.CurrentDurability = 0;
                ResetHands();
            }
        }

        public void Use(InputAction.CallbackContext context)
        {
            if(!CurrentlyEquipedItem())
                return;
            
            if(context.performed)
            {
                interactable?.Interact();
                // if(itemSO.itemType == ItemSO.ItemType.Gasoline)
                //     RemoveItem();
            }
        }
        
        private void RemoveItem()
        {
            if (!CurrentlyEquipedItem())
                return;
            currentlyequipedItem.gameObject.SetActive(false);
            ResetHands();
        }

        private bool CurrentlyEquipedItem()
        {
            if (currentlyequipedItem == null)
                return false;
            return true;
        }
        
        private void ResetHands()
        {
            itemSO = null;
            interactable = null;
            currentlyequipedItem = null;
        }
    }
}
