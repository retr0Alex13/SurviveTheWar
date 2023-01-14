using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWorkbench : MonoBehaviour
{
    [SerializeField] private Image recipieImage;
    [SerializeField] private List<CraftingRecipieSO> craftingRecipieSOList;
    [SerializeField] private BoxCollider placeItemsArea;
    [SerializeField] private Transform itemSpawnPoint;

    private CraftingRecipieSO craftingRecipieSO;

    private void Start()
    {
        NextRecipie();
    }

    public void NextRecipie()
    {
        if (craftingRecipieSO == null)
        {
            craftingRecipieSO = craftingRecipieSOList[0];
        }
        else
        {
            int index = craftingRecipieSOList.IndexOf(craftingRecipieSO);
            index = (index + 1) % craftingRecipieSOList.Count;
            craftingRecipieSO = craftingRecipieSOList[index];
            Debug.Log("NextRecipie");

        }
        recipieImage.sprite = craftingRecipieSO.craftingSprite;
    }

    public void Craft()
    {
        Collider[] colliderArray = Physics.OverlapBox(
            transform.position + placeItemsArea.center, placeItemsArea.size,
            placeItemsArea.transform.rotation);

        List<ItemSO> inputItemList = new List<ItemSO>(craftingRecipieSO.inputItemSOList);
        List<GameObject> consumeItemGOList = new List<GameObject>();
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out ItemSOHolder itemSOHolder))
            {
                if (inputItemList.Contains(itemSOHolder.itemSO))
                {
                    inputItemList.Remove(itemSOHolder.itemSO);
                    consumeItemGOList.Add(collider.gameObject);
                }
                    
            }
        }

        if (inputItemList.Count == 0)
        {
            //Have all required items to craft
            Debug.Log("Crafted!");
            Instantiate(craftingRecipieSO.outputItemSO.prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);

            foreach(GameObject consumeItemGO in consumeItemGOList)
            {
                Destroy(consumeItemGO);
            }   
        }
    }
}
