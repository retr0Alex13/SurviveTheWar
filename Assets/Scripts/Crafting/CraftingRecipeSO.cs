using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    [CreateAssetMenu(menuName = "ScriptableObjects/RecipieSO")]
    public class CraftingRecipeSO : ScriptableObject
    {
        public Sprite craftingSprite;
        public List<ItemSO> inputItemSOList;
        public ItemSO outputItemSO;
    }
}