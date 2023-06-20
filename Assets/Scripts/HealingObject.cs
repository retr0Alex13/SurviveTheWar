using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class HealingObject : MonoBehaviour, ISelectable
    {
        [SerializeField] private int healAmount;
        [SerializeField] private float sanityAmount;

        public delegate void SanityHealingAction(float sanityAmount);
        public static event SanityHealingAction OnSanityHeal;

        private PlayerHealth playerHealth;
        private Outline outline;

        private void Awake()
        {
            outline = GetComponent<Outline>();
        }
        public void Highlight()
        {
            if (outline == null) return;
            outline.enabled = true;
        }

        public void Dehighlight()
        {
            if (outline == null) return;
            outline.enabled = false;
        }

        public void Heal()
        {
            OnSanityHeal?.Invoke(sanityAmount);

            if (playerHealth == null)
            {
                playerHealth = FindFirstObjectByType<PlayerHealth>();
            }

            playerHealth.health.HealEntity(healAmount);

            // play sounds
            SoundManager.Instance.PlaySound("Healing");
        }
    }
}
