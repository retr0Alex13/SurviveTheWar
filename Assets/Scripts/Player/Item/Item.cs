using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public enum ItemType 
    {
        None,
        Scrap,
        Metal,
        Wires,
        Bolt,
        Bolts,
        Battery,
        Batteries,
        Flashlight,
        LightBulb,
    }


    public ItemSO itemScriptableObject;

    //public bool IsStackable() {
    //    return true;// IsStackable(itemType);
    //}


    //public static bool IsStackable(ItemType itemType) {
    //    switch (itemType) {
    //    default:
    //    case ItemType.Coin:
    //    case ItemType.HealthPotion:
    //    case ItemType.ManaPotion:
    //        return true;
    //    case ItemType.Sword:
    //    case ItemType.SwordNone:
    //        return false;

    //    case ItemType.Wood:
    //    case ItemType.Planks:
    //        return true;
    //    case ItemType.Sword_Diamond:
    //    case ItemType.Sword_Wood:
    //        return false;
    //    }
    //}

    //public int GetCost() {
    //    return 0;// GetCost(itemType);
    //}

    //public static int GetCost(ItemType itemType) {
    //    switch (itemType) {
    //    default:
    //    case ItemType.ArmorNone:        return 0;
    //    case ItemType.Armor_1:          return 30;
    //    case ItemType.Armor_2:          return 100;
    //    case ItemType.HelmetNone:       return 0;
    //    }
    //}

    //public override string ToString() {
    //    return itemScriptableObject.itemName;
    //}

}
