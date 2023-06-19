using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class PlayerLightDetector : MonoBehaviour
    {
        [SerializeField] private int lightPoints;
        private PlayerNeeds playerNeeds;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                playerNeeds = other.gameObject.GetComponent<PlayerNeeds>();
                playerNeeds.lightPoints = lightPoints;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            playerNeeds = other.gameObject.GetComponent<PlayerNeeds>();
            playerNeeds.lightPoints = 0;
        }

        private void OnDisable()
        {
            if (playerNeeds != null)
            {
                playerNeeds.lightPoints = 0;
            }
        }
    }
}
