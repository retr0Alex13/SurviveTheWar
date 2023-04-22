using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class InventoryMediator : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private Transform playerDropPoint;

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

        public void RemoveItemFromInventory(ItemSO item)
        {
            InventorySystem.Remove(item);
        }

        public void RemoveItemAndDrop(ItemSO item)
        {
            InventorySystem.Remove(item);
            GameObject dropItem = Instantiate(item.Prefab, 
                new Vector3(playerDropPoint.position.x, playerDropPoint.position.y, playerDropPoint.position.z), 
                Quaternion.identity);
        }

        public void InventoryVisibility(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                inventoryView.gameObject.SetActive(!isInventoryActive());
                if (isInventoryActive())
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
        private bool isInventoryActive()
        {
            return inventoryView.gameObject.activeSelf;
        }

    }
}