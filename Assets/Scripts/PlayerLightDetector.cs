using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class PlayerLightDetector : MonoBehaviour
    {
        [SerializeField] private int lightPoints;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerNeeds>().lightPoints = lightPoints;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<PlayerNeeds>().lightPoints = 0;
        }
    }
}
