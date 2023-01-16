using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEatable : MonoBehaviour
{
    [SerializeField] private float foodToRestore;
    [SerializeField] private float thirstToRestore;

    public float FoodToRestore => foodToRestore;
    public float ThirstToRestore => thirstToRestore;

}
