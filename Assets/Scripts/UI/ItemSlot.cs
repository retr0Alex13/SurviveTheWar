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

        [HideInInspector] public float ItemDurability;

        private InventoryView inventoryView;
        private PlayerEquipSlot equipSlot;

        [HideInInspector] public InventoryItem InventoryItem;

        private void Awake()
        {
            equipSlot = FindAnyObjectByType<PlayerEquipSlot>();
        }

        private void OnDestroy()
        {

        }

        public void Set(InventoryItem item)
        {
            InventoryItem = item;
            itemImage.sprite = item.itemData.image;
            inventoryView = transform.parent.GetComponent<InventoryView>();
            
            if (item.itemData.itemType == ItemSO.ItemType.Gasoline)
            {
                Equip();
                RemoveWithoutDrop();
                return;
            }

            if (item.itemData.itemType == ItemSO.ItemType.Eatable)
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
            inventoryView.inventoryMediator.RemoveItemAndDrop(InventoryItem.itemData, ItemDurability);
        }

        public void RemoveWithoutDrop()
        {
            inventoryView.inventoryMediator.RemoveItemFromInventory(InventoryItem.itemData);
        }

        public void SetWindowState()
        {
            OptionPanel.SetActive(!OptionPanel.activeSelf);
        }

        public void Use()
        {
            if (InventoryItem.itemData.Prefab.TryGetComponent(out ObjectEatable objectEatable))
            {
                objectEatable.Consume();
                inventoryView.inventoryMediator.RemoveItemFromInventory(InventoryItem.itemData);
            }
        }

        public void Equip()
        {
            if (InventoryItem.itemData.Prefab.TryGetComponent(out IInteractable interactable))
            {
                if (equipSlot.currentlyequipedItem != null)
                    return;
                ItemSOHolder itemSOHolder = InventoryItem.itemData.Prefab.GetComponent<ItemSOHolder>();
                itemSOHolder.CurrentDurability = ItemDurability;
                equipSlot.EquipItem(itemSOHolder);
                inventoryView.inventoryMediator.RemoveItemFromInventory(InventoryItem.itemData);
            }
        }
    }
}