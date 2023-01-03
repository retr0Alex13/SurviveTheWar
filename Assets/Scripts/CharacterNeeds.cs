using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNeeds : MonoBehaviour
{
    [Header("Hunger")]
    [SerializeField] float maxHunger = 100f;
    [SerializeField] float decreaseHungerRate = 0.1f;
    float currentHungerValue;
    public float HungerPercent => currentHungerValue / maxHunger;

    [Header("Thirst")]
    [SerializeField] private float maxThirst = 100f;
    [SerializeField] private float decreaseThirstRate = 0.1f;
    float currentThirstValue;
    public float ThirstPercent => currentThirstValue / maxThirst;

    [Header("Stamina")]
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float decreaseStaminaRate = 0.1f;
    [SerializeField] float rechargeStaminaRate = 1.5f;
    [SerializeField] float rechargeStaminaDelay = 1f;
    float currentStaminaValue;
    float currentStaminaDelayCounter;
    public float Stamina => currentStaminaValue / maxStamina;

    [Header("Player Refernces")]
    StarterAssetsInputs starterAssetsInputs;

    void Awake()
    {
       starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Start()
    {
        currentThirstValue = maxThirst;
        currentHungerValue = maxHunger;
        currentStaminaValue = maxStamina;
    }

    void Update()
    {
        currentHungerValue -= decreaseHungerRate * Time.deltaTime;
        currentThirstValue -= decreaseHungerRate * Time.deltaTime;
    }

}
