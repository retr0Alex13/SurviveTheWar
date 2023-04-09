using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace OM
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private GameObject amountObject;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject OptionPanel;

        [HideInInspector] public InventoryItem InventoryItem;

        public void Set(InventoryItem item)
        {
            InventoryItem = item;
            itemImage.sprite = item.itemData.image;

            if(item.StackSize <= 1)
            {
                amountObject.SetActive(false);
                return;
            }
            amountText.text = item.StackSize.ToString();
        }

        public void Remove()
        {
            InventoryView inventoryView = transform.parent.GetComponent<InventoryView>();
            inventoryView.RemoveItemSlot(InventoryItem);
        }

        public void SetState()
        {
            OptionPanel.SetActive(!OptionPanel.activeSelf);
        }

    }
}