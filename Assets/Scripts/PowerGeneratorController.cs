using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class PowerGeneratorController : MonoBehaviour, IInteractable
    {
        public bool IsPickable { get; set; }

        [SerializeField] private ParticleSystem smokeParticle;
        [SerializeField] private float fuel = 30f;
        [SerializeField] private float maxFloatCapacity = 100;
        [SerializeField] private List<Light> lights;
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
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
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
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }

        public void AddFuel(float fuelAmount)
        {
            fuel += fuelAmount;
            fuel = Mathf.Clamp(fuel, 0, maxFloatCapacity);
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
