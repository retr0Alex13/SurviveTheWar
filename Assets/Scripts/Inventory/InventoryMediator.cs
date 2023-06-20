﻿using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class InventoryMediator : MonoBehaviour
    {
        [SerializeField] public InventoryView inventoryView;
        [SerializeField] private TaskWindowBuilder taskWindow;
        [SerializeField] private GameObject inventoryMenu;
        [SerializeField] private Transform playerDropPoint;

        private FirstPersonController firstPersonController;

        public delegate void InventoryMediatorAction(bool status);
        public static event InventoryMediatorAction OnInventoryOpened;

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

            firstPersonController = GetComponent<FirstPersonController>();
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

        public void AddItemToInventory(ItemSO item, ItemSOHolder itemSOHolder)
        {
            if(itemSOHolder.TryGetComponent(out ItemDurability itemDurability))
            {
                Debug.Log(item.itemName + " durability when addding to inventory: " + itemDurability.CurrentDurability);

                InventorySystem.Add(item, itemDurability);
            }
            else
            {
                InventorySystem.Add(item, null);

            }
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
                    OnInventoryOpened?.Invoke(true);
                    firstPersonController.enabled = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    taskWindow.CloseWindow();
                    OnInventoryOpened?.Invoke(false);
                    firstPersonController.enabled = true;
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