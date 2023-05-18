using UnityEngine;

namespace OM
{
    public class HealthSystem
    {
        private int currentHealth;
        private int currentMaxHealth;
        public int CurrentMaxHealth
        {
            get { return currentMaxHealth; }
        }
        public int CurrentHealth
        {
            get { return currentHealth; }
        }

        public delegate void OnHealthChange(float currentMaxHealth, float currentHealth);
        public static event OnHealthChange OnHealthChanged;

        private float damageCounter;
        private float damageRate = 3f;

        private float healCounter;
        private float healRate = 2f;


        public HealthSystem(int health, int maxHealth)
        {
            currentHealth = health;
            currentMaxHealth = maxHealth;
        }

        public void DamageEntity(int damageAmount)
        {
            if (currentHealth > 0)
            {
                damageCounter += Time.deltaTime;
                if (damageCounter >= damageRate)
                {
                    currentHealth -= damageAmount;
                    if (OnHealthChanged != null)
                    {
                        OnHealthChanged(CurrentMaxHealth, CurrentHealth);
                    }
                    damageCounter = 0;
                }
            }
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }
        public void HealEntity(int healAmount)
        {
            if (currentHealth < currentMaxHealth)
            {
                healCounter += Time.deltaTime;
                if (healCounter >= healRate)
                {
                    currentHealth += healAmount;
                    if (OnHealthChanged != null)
                    {
                        OnHealthChanged(CurrentMaxHealth, CurrentHealth);
                    }
                    healCounter = 0;
                }
            }
            if (currentHealth > currentMaxHealth)
            {
                currentHealth = currentMaxHealth;
            }
        }
    }
}