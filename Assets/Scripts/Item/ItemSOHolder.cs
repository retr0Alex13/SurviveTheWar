using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class ItemSOHolder : MonoBehaviour
    {
        [SerializeField] private ItemSO itemSO;

        public ItemSO ItemSO { get { return itemSO; } }
    }
}