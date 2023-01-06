using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterNeeds : MonoBehaviour
{
    [Header("Hunger")]
    [SerializeField] float maxHunger = 100f;
    [SerializeField] float decreaseHungerRate = 0.1f;
    [SerializeField] float currentHunger;
    public float HungerPercent => currentHunger / maxHunger;

    [Header("Thirst")]
    [SerializeField] private float maxThirst = 100f;
    [SerializeField] private float decreaseThirstRate = 0.1f;
    [SerializeField] float currentThirst;
    public float ThirstPercent => currentThirst / maxThirst;

    [Header("Stamina")]
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float decreaseStaminaRate = 0.1f;
    [SerializeField] float rechargeStaminaRate = 1.5f;
    [SerializeField] float rechargeStaminaDelay = 1f;
    float currentStamina;
    float currentStaminaDelayCounter;
    public float Stamina => currentStamina / maxStamina;

    [Header("Player Refernces")]
    StarterAssetsInputs playerInputs;

    void Awake()
    {
        playerInputs = GetComponent<StarterAssetsInputs>();
    }

    void Start()
    {
        currentThirst = maxThirst;
        currentHunger = maxHunger;
        currentStamina = maxStamina;
    }

    void Update()
    {
        currentHunger -= decreaseHungerRate * Time.deltaTime;
        currentThirst -= decreaseThirstRate * Time.deltaTime;

        if (currentHunger <= 0 || currentThirst <= 0)
        {
            GameManager.gameManager.playerHealth.DamageEntity(5);
            currentHunger = 0;
            currentThirst = 0;
        }

        if (playerInputs.IsSprinting())
        {
            currentStamina -= decreaseStaminaRate * Time.deltaTime;
        }

        if (!playerInputs.IsSprinting() && currentStamina < maxStamina)
        {
            if (currentStaminaDelayCounter < rechargeStaminaDelay)
            {
                currentStamina += Time.deltaTime;
            }
            if (currentStaminaDelayCounter >= rechargeStaminaDelay)
            {
                currentStamina += rechargeStaminaRate * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }
        }
    }

    void AddHungerAndThirst(float hungerAmount, float thirstAmount)
    {

    }

}
