using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class InventoryMediator : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private TaskWindowBuilder taskWindow;
        [SerializeField] private GameObject inventoryMenu;
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
        
        public bool IsInventoryFull(ItemSO item)
        {
            if (InventorySystem.Get(item) == null)
            {
                if (InventorySystem.Inventory.Count == InventorySystem.slotsLimit)
                {
                    SoundManager.Instance.PlaySound("FullInventory");
                    return true;
                }
            }
            return false;
        }

        public void AddItemToInventory(ItemSO item)
        {
            InventorySystem.Add(item);
            EvaluateGatheringGoal(item.itemName);
        }

        public void RemoveItemFromInventory(ItemSO item)
        {
            InventorySystem.Remove(item);
        }

        public void RemoveItemAndDrop(ItemSO item)
        {
            if(InventorySystem.Get(item) != null)
            {
                InventorySystem.Remove(item);
            }
            GameObject dropItem = Instantiate(item.Prefab, 
                new Vector3(playerDropPoint.position.x, playerDropPoint.position.y, playerDropPoint.position.z), 
                Quaternion.identity);
        }

        public void InventoryVisibility(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                inventoryMenu.SetActive(!isInventoryMenuActive());
                if (isInventoryMenuActive())
                {
                    Cursor.lockState = CursorLockMode.None;
                    SoundManager.Instance.PlaySound("OpenInventory");
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    taskWindow.CloseWindow();
                }
            }
        }
        private bool isInventoryMenuActive()
        {
            return inventoryMenu.activeSelf;
        }
        public void EvaluateGatheringGoal(string itemName)
        {
            EventManager.Instance.QueueEvent(new GatheringGameEvent(itemName));
        }
    }
}