using System.Collections;
using System.Collections.Generic;
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
        private IInteractable interactable;

        public void EquipItem(ItemSO itemData)
        {
            itemSO = itemData;
            foreach(Transform transform in equipSlot)
            {
                ItemSO item = transform.GetComponent<ItemSOHolder>().ItemSO;
                if (item == itemSO)
                {
                    currentlyequipedItem = transform;
                    currentlyequipedItem.gameObject.SetActive(true);
                    interactable = currentlyequipedItem.GetComponent<IInteractable>();
                }
            }
        }

        public void UnEquipItem(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                if (currentlyequipedItem == null)
                    return;

                currentlyequipedItem.gameObject.SetActive(false);
                GameObject dropItem = Instantiate(itemSO.Prefab, new Vector3(dropItemPoint.position.x, dropItemPoint.position.y, dropItemPoint.position.z),
                Quaternion.identity);
                itemSO = null;
                interactable = null;
                currentlyequipedItem = null;
            }
        }

        public void Use(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                interactable?.Interact();
            }
        }
    }
}
