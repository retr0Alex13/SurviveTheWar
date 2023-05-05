using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerEquipSlot : MonoBehaviour
    {
        [SerializeField] private Transform equipSlot;

        [HideInInspector] public ItemSO itemSO;
        private InventoryMediator inventoryMediator;
        private Transform currentlyequipedItem;

        private void Awake()
        {
            inventoryMediator = GetComponent<InventoryMediator>();
        }

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
                }
            }
        }

        public void UnEquipItem(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                if (itemSO == null)
                    return;
                inventoryMediator.RemoveItemAndDrop(itemSO);

                currentlyequipedItem.gameObject.SetActive(false);
                itemSO = null;
            }
        }
    }
}
