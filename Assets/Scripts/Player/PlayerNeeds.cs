using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerNeeds : MonoBehaviour
    {
        [SerializeField] private int starvingDamagePerTick = 5;

        #region Hunger
        [Header("Hunger")]
        [SerializeField] private float maxHunger = 100f;

        [SerializeField] private float decreaseHungerRate = 1f;
        [SerializeField] private float currentHunger;
        public float HungerPercent => currentHunger / maxHunger;
        #endregion

        #region Thirst
        [Header("Thirst")]
        [SerializeField] private float maxThirst = 100f;

        [SerializeField] private float decreaseThirstRate = 1f;
        [SerializeField] private float currentThirst;
        public float ThirstPercent => currentThirst / maxThirst;
        #endregion

        #region Sanity
        [Header("Sanity")]
        [SerializeField] private float maxSanity = 100f;

        [SerializeField] private float decreaseSanityRate = 1f;
        [SerializeField] private float currentSanity;
        public float SanityPercent => currentSanity / maxSanity;
        #endregion

        #region Stamina
        [Header("Stamina")]
        [SerializeField] private float maxStamina = 100f;

        [SerializeField] private float decreaseStaminaRate = 1f;

        [SerializeField] private float rechargeStaminaRate = 2f;
        [SerializeField] private float rechargeStaminaDelay = 1f;

        [SerializeField] private float currentStamina;
        [SerializeField] private float currentStaminaDelayCounter;

        public float StaminaPercent => currentStamina / maxStamina;
        #endregion

        public delegate void CharacterStaminaAction(bool HasStamina);

        public static event CharacterStaminaAction OnExhausted;

        public delegate void CharacterNeedsAction(
            float Hunger, 
            float Thirst, 
            float Stamina);

        public static event CharacterNeedsAction OnNeedsChanged;

        [Header("Player Refernces")]
        [SerializeField] private Transform playerCameraTransform;
        private StarterAssetsInputs playerInputs;
        private PlayerHealth playerHealth;

        private void OnEnable()
        {
            ObjectEatable.OneItemConsuming += AddHungerAndThirst;
        }

        private void OnDisable()
        {
            ObjectEatable.OneItemConsuming -= AddHungerAndThirst;
        }

        private void Start()
        {
            currentThirst = maxThirst;
            currentHunger = maxHunger;
            currentStamina = maxStamina;
            
            playerInputs = GetComponent<StarterAssetsInputs>();
            playerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            HandleStarvingAndThirst();
            UpdateStamina();
            OnNeedsChanged?.Invoke(HungerPercent, ThirstPercent, StaminaPercent);
        }

        private void HandleStarvingAndThirst()
        {
            currentHunger -= decreaseHungerRate * Time.deltaTime;
            currentThirst -= decreaseThirstRate * Time.deltaTime;

            if (currentHunger <= 0 || currentThirst <= 0)
            {
                playerHealth.playerHealth.DamageEntity(starvingDamagePerTick);

                currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
                currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
            }
        }

        private void UpdateStamina()
        {
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            if (PlayerSprinting())
            {
                HandleSprinting();
            }
            else if (currentStamina < maxStamina)
            {
                HandleRecharging();
            }
        }

        private bool PlayerSprinting()
        {
            return playerInputs.IsSprinting() && playerInputs.GetMove().y > 0.1f;
        }

        private void HandleSprinting()
        {
            currentStamina -= decreaseStaminaRate * Time.deltaTime;
            if (currentStamina <= 0)
            {
                OnExhausted(false);
                currentStamina = 0;
            }
            currentStaminaDelayCounter = 0f;
        }

        private void HandleRecharging()
        {
            if (currentStaminaDelayCounter < rechargeStaminaDelay)
            {
                currentStaminaDelayCounter += Time.deltaTime;
            }
            else
            {
                currentStamina += rechargeStaminaRate * Time.deltaTime;
            }
        }

        private void AddHungerAndThirst(float hungerAmount, float thirstAmount)
        {
            currentHunger += hungerAmount;
            currentThirst += thirstAmount;

            currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
            currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
        }
    }
}