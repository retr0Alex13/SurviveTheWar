using OM;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Inventory inventory;
    private Transform inventoryPanel;

    private void OnEnable()
    {
        Inventory.OnItemChanged += UpdateInventoryUI;
    }

    private void OnDisable()
    {
        Inventory.OnItemChanged -= UpdateInventoryUI;
    }

    private void Start()
    {
        inventoryPanel = GetComponent<Transform>();
        UpdateInventoryUI();
    }

    private void Update()
    {

    }

    public void UpdateInventoryUI()
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


}
