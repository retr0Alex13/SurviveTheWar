using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerNeeds : MonoBehaviour
    {
        [SerializeField] private int starvingDamagePerTick = 5;
        [SerializeField] private int onFullNeedsHealPoints = 10;

        #region Hunger
        
        [Header("Hunger")]
        [SerializeField] private float maxHunger = 100f;

        [SerializeField] private float decreaseHungerRate = 1f;
        [SerializeField] private float currentHunger;
        
        [SerializeField, Tooltip("Used to simulate short time when player is not hungry")]
        private int bonusSatedPoints = 10;
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
        [SerializeField] private float hightDecreaseSanityRate = 5f;
        [SerializeField] private float currentSanity;

        [SerializeField] private int sanityDamage = 10;

        [SerializeField] public int lightPoints = 0;
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

        [SerializeField] private DayNightCycle dayNightCycle;
        
        public delegate void CharacterStaminaAction(bool HasStamina);
        public static event CharacterStaminaAction OnExhausted;

        public delegate void CharacterNeedsAction(
            float Hunger, 
            float Thirst, 
            float Stamina);

        public static event CharacterNeedsAction OnNeedsChanged;

        public delegate void CharacterSanityAction(float maxSanity, float currentSanity);
        public static event CharacterSanityAction OnSanityChanged;
        

        [Header("Player Refernces")]
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
            currentSanity = maxSanity;
            
            playerInputs = GetComponent<StarterAssetsInputs>();
            playerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            HandleStarvingAndThirst();
            UpdateStamina();
            HandleSanity();


            OnNeedsChanged?.Invoke(HungerPercent, ThirstPercent, StaminaPercent);
        }

        private void HandleSanity()
        {
            if (!dayNightCycle.IsNight())
            {
                return;
            }
            if(lightPoints == 100)
                return;
            if (lightPoints < 100)
            {
                ReduceSanity(decreaseSanityRate);
            }
            else if (lightPoints <= 0)
            {
                ReduceSanity(hightDecreaseSanityRate);
            }

            if (currentSanity <= 0)
            {
                // Damage player for 0 sanity
                playerHealth.health.DamageEntity(sanityDamage);
                currentSanity = Mathf.Clamp(currentSanity, 0f, maxStamina);
            }
            OnSanityChanged?.Invoke(maxSanity, currentSanity);
            //Debug.Log("Sanity: " + currentSanity);
        }

        private void HandleStarvingAndThirst()
        {
            currentHunger -= decreaseHungerRate * Time.deltaTime;
            currentThirst -= decreaseThirstRate * Time.deltaTime;

            if (currentHunger <= 0 || currentThirst <= 0)
            {
                playerHealth.health.DamageEntity(starvingDamagePerTick);

                currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
                currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
            }
            
            if (currentHunger >= maxHunger && currentThirst >= maxThirst)
            {
                playerHealth.health.HealEntity(onFullNeedsHealPoints);
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
                HandleStaminaRecharging();
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

        private void HandleStaminaRecharging()
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

            currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger + bonusSatedPoints);
            currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst + bonusSatedPoints);
        }

        public void ReduceSanity(float valueAmount)
        {
            currentSanity -= valueAmount * Time.deltaTime;
        }

        public void AddSanity(float valueAmount)
        {
            // Add Sanity
        }
    }
}