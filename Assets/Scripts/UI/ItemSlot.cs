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

        private InventoryView inventoryView;

        [HideInInspector] public InventoryItem InventoryItem;

        public void Set(InventoryItem item)
        {
            InventoryItem = item;
            itemImage.sprite = item.itemData.image;
            inventoryView = transform.parent.GetComponent<InventoryView>();

            if(item.itemData.itemType == ItemSO.ItemType.Eatable)
            {
                useButton.SetActive(true);
            }
            else
            {
                useButton.SetActive(false);
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
            inventoryView.inventoryMediator.RemoveItemAndDrop(InventoryItem.itemData);
        }

        public void SetWindowState()
        {
            OptionPanel.SetActive(!OptionPanel.activeSelf);
        }

        public void Use()
        {
            if(InventoryItem.itemData.Prefab.TryGetComponent(out ObjectEatable objectEatable))
            {
                objectEatable.Consume();
                inventoryView.inventoryMediator.RemoveItemFromInventory(InventoryItem.itemData);
            }
        }
    }
}