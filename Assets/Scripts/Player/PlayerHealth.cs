using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerHealth : MonoBehaviour
    {
        public HealthSystem health = new HealthSystem(100, 100);
        public delegate void PlayerHealthAction();
        public static event PlayerHealthAction OnPlayerDead;
        
        private PlayerInput playerInput;

        private void Awake() => playerInput = GetComponent<PlayerInput>();

        private void OnEnable()
        {
            HealthSystem.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            HealthSystem.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float currentMaxHealth, float currentHealth)
        {
            if (health.CurrentHealth > 0)
            {
                return;
            }
            PlayerDead();
        }

        private void PlayerDead()
        {
            playerInput.enabled = false;
            OnPlayerDead();
        }
    }
}
