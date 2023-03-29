using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OM
{
    public class CraftingStation : MonoBehaviour, IInteractable
    {
        [SerializeField] private Image recipieImage;
        [SerializeField] private List<CraftingRecipeSO> craftingRecipeSOList;
        [SerializeField] private BoxCollider placeItemsArea;
        [SerializeField] private Transform itemSpawnPoint;

        private CraftingRecipeSO craftingRecipieSO;
        private Outline outline;

        private void Start()
        {
            outline = GetComponent<Outline>();
            NextRecipe();
        }

        public void Highlight()
        {
            if (outline == null) return;
            outline.enabled = true;
        }

        public void Dehighlight()
        {
            if (outline == null) return;
            outline.enabled = false;
        }

        public void NextRecipe()
        {
            if (craftingRecipieSO == null)
            {
                craftingRecipieSO = craftingRecipeSOList[0];
            }
            else
            {
                int index = craftingRecipeSOList.IndexOf(craftingRecipieSO);
                index = (index + 1) % craftingRecipeSOList.Count;
                craftingRecipieSO = craftingRecipeSOList[index];
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
                    if (inputItemList.Contains(itemSOHolder.ItemSO))
                    {
                        inputItemList.Remove(itemSOHolder.ItemSO);
                        consumeItemGOList.Add(collider.gameObject);
                    }

                }
            }

            if (inputItemList.Count == 0)
            {
                //Craft needed item
                Debug.Log("Crafted!");
                EvaluateCraftingGoal(craftingRecipieSO.outputItemSO.name);
                Instantiate(craftingRecipieSO.outputItemSO.Prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);


                foreach (GameObject consumeItemGO in consumeItemGOList)
                {
                    Destroy(consumeItemGO);
                }
            }
        }

        public void EvaluateCraftingGoal(string itemName)
        {
            EventManager.Instance.QueueEvent(new CraftingGameEvent(itemName));
        }
    }
}