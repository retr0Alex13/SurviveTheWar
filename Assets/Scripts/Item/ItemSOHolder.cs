using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSOHolder : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;

    public ItemSO ItemSO { get { return itemSO; } }
}
