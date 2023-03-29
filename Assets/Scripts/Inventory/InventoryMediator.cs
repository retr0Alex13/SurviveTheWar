using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class InventoryMediator : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;

        public InventorySystem InventorySystem = new InventorySystem();


        void OnEnable()
        {
            InventorySystem.OnInventoryChange += UpdateInventory;
            PlayerPickupDrop.OnItemPickUp += AddItemToInventory;
        }

        void OnDisable()
        {
            InventorySystem.OnInventoryChange -= UpdateInventory;
            PlayerPickupDrop.OnItemPickUp -= AddItemToInventory;
        }

        private void Awake()
        {
            InventorySystem.Inventory = new List<InventoryItem>();
            InventorySystem.ItemDictionary = new Dictionary<ItemSO, InventoryItem>();
        }

        public void UpdateInventory()
        {
            inventoryView.OnUpdateInventory();
        }

        public void AddItemToInventory(ItemSO item)
        {
            InventorySystem.Add(item);
        }

        public void InventoryVisibility(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                inventoryView.gameObject.SetActive(!isInventoryActive());
            }
        }

        private bool isInventoryActive()
        {
            return inventoryView.gameObject.activeSelf;
        }

    }
}