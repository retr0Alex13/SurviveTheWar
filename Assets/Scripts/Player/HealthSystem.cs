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

    float damageCounter;
    float damageRate = 5f;

    float healCounter;
    float healRate = 5f;


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
            }
            healCounter = 0;
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
