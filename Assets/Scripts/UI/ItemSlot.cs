using System.Collections;
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

        public void Set(InventoryItem item)
        {
            itemImage.sprite = item.itemData.image;
            if(item.StackSize <= 1)
            {
                amountObject.SetActive(false);
                return;
            }

            amountText.text = item.StackSize.ToString();
        }
    }
}