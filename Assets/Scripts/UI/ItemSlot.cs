using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OM
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private GameObject amountObject;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject OptionPanel;
        [SerializeField] private GameObject useButton;
        [SerializeField] private GameObject equipButton;

        private InventoryView inventoryView;
        private PlayerEquipSlot equipSlot;

        private InventoryItem inventoryItem;
        public InventoryItem InventoryItem { get { return inventoryItem; } }

        private void Awake()
        {
            equipSlot = FindAnyObjectByType<PlayerEquipSlot>();
        }

        public void Set(InventoryItem item)
        {
            inventoryItem = item;
            itemImage.sprite = item.itemData.image;
            inventoryView = transform.parent.GetComponent<InventoryView>();
            Debug.Log(inventoryItem.ItemDurability);
            if (inventoryItem.ItemDurability != null)
            {
                Debug.Log(inventoryItem.itemData.itemName + " durability " + inventoryItem.ItemDurability.CurrentDurability);
            }

            if (item.itemData.itemType == ItemSO.ItemType.Gasoline)
            {
                Equip();
                RemoveWithoutDrop();
                return;
            }

            if (item.itemData.itemType == ItemSO.ItemType.Eatable || item.itemData.itemType == ItemSO.ItemType.Usable)
            {
                useButton.SetActive(true);
            }
            else if (item.itemData.itemType == ItemSO.ItemType.Equipable)
            {
                equipButton.SetActive(true);
            }
            if (item.StackSize <= 1)
            {
                amountObject.SetActive(false);
                return;
            }
            amountText.text = item.StackSize.ToString();
        }

        public void Remove()
        {
            inventoryView.inventoryMediator.RemoveItemAndDrop(inventoryItem.itemData);
        }

        public void RemoveWithoutDrop()
        {
            inventoryView.inventoryMediator.RemoveItemFromInventory(inventoryItem.itemData);
        }

        public void SetWindowState()
        {
            OptionPanel.SetActive(!OptionPanel.activeSelf);
        }

        public void Use()
        {
            if (inventoryItem.itemData.Prefab.TryGetComponent(out ObjectEatable objectEatable))
            {
                objectEatable.Consume();
            }

            if (inventoryItem.itemData.Prefab.TryGetComponent(out HealingObject healingObject))
            {
                healingObject.Heal();
            }

            if(inventoryItem.itemData.Prefab.TryGetComponent(out Battery battery))
            {
                if (equipSlot.currentlyequipedItem.TryGetComponent(out Flashlight flashlight1))
                {
                    battery.flashlight = flashlight1;
                    battery.Interact();
                }
                Debug.Log(equipSlot.currentlyequipedItem);
            }
            inventoryView.inventoryMediator.RemoveItemFromInventory(inventoryItem.itemData);
        }

        public void Equip()
        {
            Debug.Log(inventoryItem.ItemDurability);
            if (inventoryItem.itemData.Prefab.TryGetComponent(out IInteractable interactable))
            {
                if (equipSlot.currentlyequipedItem != null)
                    return;
                ItemSOHolder itemSOHolder = inventoryItem.itemData.Prefab.GetComponent<ItemSOHolder>();
                if (inventoryItem != null)
                {
                    equipSlot.EquipItem(itemSOHolder, inventoryItem.ItemDurability);
                }
                else
                {
                    equipSlot.EquipItem(itemSOHolder, null);

                }
                inventoryView.inventoryMediator.RemoveItemFromInventory(inventoryItem.itemData);
            }
        }
    }
}