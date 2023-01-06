using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    int currentHealth;
    int currentMaxHealth;
    public int CurrentMaxHealth => currentMaxHealth;
    public int CurrentHealth => currentHealth;

    public delegate void OnHealthChange(float currentMaxHealth, float currentHealth);
    public static event OnHealthChange OnHealthChanged;


    public HealthSystem(int health, int maxHealth)
    {
        currentHealth = health;
        currentMaxHealth = maxHealth;
    }

    public void DamageEntity(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            if (OnHealthChanged != null)
            {
                OnHealthChanged(CurrentMaxHealth, CurrentHealth);
            }
        }
    }
    public void HealEntity(int healAmount)
    {
        if (currentHealth < currentMaxHealth)
        {
            currentHealth += healAmount;
        }
        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged(CurrentMaxHealth, CurrentHealth);
        }
    }
}
