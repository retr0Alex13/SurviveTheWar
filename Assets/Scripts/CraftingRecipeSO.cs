using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RecipieSO")]
public class CraftingRecipeSO : ScriptableObject
{
    public Sprite craftingSprite;
    public List<ItemSO> inputItemSOList;
    public ItemSO outputItemSO;
}
