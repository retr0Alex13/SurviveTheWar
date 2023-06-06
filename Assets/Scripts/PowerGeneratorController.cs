using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OM
{
    public class PowerGeneratorController : MonoBehaviour, IInteractable
    {
        public bool IsPickable { get; set; }

        [SerializeField] private ParticleSystem smokeParticle;
        [SerializeField] private float fuel = 30f;
        [SerializeField] private float maxFuelCapacity = 100;
        [SerializeField] private List<GameObject> lights;
        public bool isTurnedOn;

        private void Update()
        {
            if (!isTurnedOn)
            {
                return;
            }
            else
            {
                // Start to subtract fuel;
                fuel -= Time.deltaTime / 5;
            }
            Debug.Log($"Generator fuel: {fuel}");
            if (!CheckFuel())
            {
                StopGenerator();
                fuel = 0;
            }
        }

        public void Interact()
        {
            SoundManager.Instance.PlaySound("Switch", transform.position);
            ControlGenerator();
        }
        
        private void ControlGenerator()
        {
            if (!CheckFuel() && !isTurnedOn)
            {
                Debug.Log("Can't start without fuel");
                return;
            }
            else if (CheckFuel() && !isTurnedOn)
            {
                StartGenerator();
            }
            else
            {
                StopGenerator();
            }
        }

        private void StartGenerator()
        {
            Debug.Log("Starting..");
            isTurnedOn = true;
            
            // Play particles
            if (!smokeParticle.isPlaying)
            {
                smokeParticle.Play();
            }
            
            // Play sound
            SoundManager.Instance.PlaySound("GeneratorRunning", transform.position);

            // Set light enabled = true in light list
            SetLights(true);
        }

        private void StopGenerator()
        {
            Debug.Log("Stopping generator");
            isTurnedOn = false;
            
            // Stop particles
            if (smokeParticle.isPlaying)
            {
                smokeParticle.Stop();
            }
            
            // Stop sound
            SoundManager.Instance.StopSound("GeneratorRunning");
            
            // Set light enabled = false in light list
            SetLights(false);
        }

        private void SetLights(bool status)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(status);
            }
        }

        public void AddFuel(float fuelAmount)
        {
            fuel += fuelAmount;
            fuel = Mathf.Clamp(fuel, fuel, maxFuelCapacity);
            
            SoundManager.Instance.PlaySound("PouringWater", transform.position);
            
            Debug.Log("Fuel: " + fuel);
        }

        private bool CheckFuel()
        {
            if (fuel > 0)
            {
                return true;
            }

            return false;
        }
    }
}
