using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerNeeds : MonoBehaviour
    {
        [Header("Hunger")]
        [SerializeField] float maxHunger = 100f;
        [SerializeField] float decreaseHungerRate = 1f;
        [SerializeField] float currentHunger;
        public float HungerPercent => currentHunger / maxHunger;

        [Header("Thirst")]
        [SerializeField] private float maxThirst = 100f;
        [SerializeField] private float decreaseThirstRate = 1f;
        [SerializeField] float currentThirst;
        public float ThirstPercent => currentThirst / maxThirst;

        [SerializeField] private float interactDistance = 2f;

        // TODO: Make sanity
        [Header("Sanity")]
        [SerializeField] private float maxSanity = 100f;
        [SerializeField] private float decreaseSanityRate = 1f;
        [SerializeField] float currentSanity;
        public float SanityPercent => currentSanity / maxSanity;

        [Header("Stamina")]
        [SerializeField] float maxStamina = 100f;

        [SerializeField] float decreaseStaminaRate = 1f;

        [SerializeField] float rechargeStaminaRate = 2f;
        [SerializeField] float rechargeStaminaDelay = 1f;

        [SerializeField] float currentStamina;
        [SerializeField] float currentStaminaDelayCounter;

        [SerializeField] bool isSprinting = false;
        [SerializeField] bool HasStamina = true;
        public float StaminaPercent => currentStamina / maxStamina;

        public delegate void CharacterStaminaAction(bool HasStamina);
        public static event CharacterStaminaAction OnExhausted;

        public delegate void CharacterNeedsAction(float Hunger, float Thirst, float Stamina);
        public static event CharacterNeedsAction OnNeedsChanged;

        [Header("Player Refernces")]
        [SerializeField] private Transform playerCameraTransform;

        void Start()
        {
            currentThirst = maxThirst;
            currentHunger = maxHunger;
            currentStamina = maxStamina;
        }

        void Update()
        {
            HandleStarvingAndThirst();
            HandleStamina();
            OnNeedsChanged?.Invoke(HungerPercent, ThirstPercent, StaminaPercent);
        }

        public void HandleEatingOrDrinking(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, interactDistance))
                {
                    if (raycastHit.transform.TryGetComponent(out ObjectEatable itemEatable))
                    {
                        AddHungerAndThirst(itemEatable.FoodToRestore, itemEatable.ThirstToRestore);
                        Destroy(itemEatable.gameObject);
                    }
                }
            }
        }

        private void HandleStarvingAndThirst()
        {
            currentHunger -= decreaseHungerRate * Time.deltaTime;
            currentThirst -= decreaseThirstRate * Time.deltaTime;


            if (currentHunger <= 0 || currentThirst <= 0)
            {
                GameManager.gameManager.playerHealth.DamageEntity(5);

                currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
                currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
            }
        }

        private void HandleStamina()
        {
            if (isSprinting)
            {
                currentStamina -= decreaseStaminaRate * Time.deltaTime;
                if (currentStamina <= 0)
                {
                    HasStamina = false;
                    OnExhausted(HasStamina);
                    currentStamina = 0;
                }
                else
                {
                    HasStamina = true;
                    OnExhausted(HasStamina);
                }
                currentStaminaDelayCounter = 0;
            }

            if (!isSprinting && currentStamina < maxStamina)
            {
                Debug.Log("not sprinting and stamina is low");
                if (currentStaminaDelayCounter < rechargeStaminaDelay)
                {
                    currentStaminaDelayCounter += Time.deltaTime;
                }
                if (currentStaminaDelayCounter >= rechargeStaminaDelay)
                {
                    currentStamina += rechargeStaminaRate * Time.deltaTime;
                }
            }
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        public void SprintInput(InputAction.CallbackContext ctx)
        {
            isSprinting = ctx.ReadValueAsButton();
        }

        void AddHungerAndThirst(float hungerAmount, float thirstAmount)
        {
            currentHunger += hungerAmount;
            currentThirst += thirstAmount;

            currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
            currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
        }

    }
}