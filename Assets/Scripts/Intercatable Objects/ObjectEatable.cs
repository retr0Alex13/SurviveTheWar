using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class ObjectEatable : MonoBehaviour
    {
        [SerializeField] private float foodToRestore;
        [SerializeField] private float thirstToRestore;

        public float FoodToRestore { get { return foodToRestore; } }
        public float ThirstToRestore { get { return thirstToRestore; } }

    }
}