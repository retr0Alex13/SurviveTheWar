using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace OM
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryItemPrefab;
        [SerializeField] private Inventory inventory;

        /// <summary>
        /// Updates Inventory UI every time inventory changed.
        /// Called in <see cref="InventoryMediator"/>
        /// </summary>
        /// <param name="inventoryPanel"></param>
        public void UpdateInventoryUI(Transform inventoryPanel)
        {
            // Remove any existing inventory items from the panel
            foreach (Transform child in inventoryPanel.transform)
            {
                if (!child.gameObject)
                    return;
                Destroy(child.gameObject);
            }

            // Iterate over the list of inventory items and instantiate the UI prefab for each one
            foreach (ItemSO item in inventory.items)
            {
                GameObject newItem = Instantiate(inventoryItemPrefab, inventoryPanel);
                newItem.transform.GetComponentInChildren<Image>().sprite = item.icon;
                newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
            }
        }

        public void ControlInventoryWindow(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
}
