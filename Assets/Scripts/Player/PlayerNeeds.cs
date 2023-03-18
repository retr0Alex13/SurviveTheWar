using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class PlayerNeeds : MonoBehaviour
    {
        [Header("Hunger")]
        [SerializeField] private float maxHunger = 100f;

        [SerializeField] private float decreaseHungerRate = 1f;
        [SerializeField] private float currentHunger;
        public float HungerPercent => currentHunger / maxHunger;

        [Header("Thirst")]
        [SerializeField] private float maxThirst = 100f;

        [SerializeField] private float decreaseThirstRate = 1f;
        [SerializeField] private float currentThirst;
        public float ThirstPercent => currentThirst / maxThirst;

        [SerializeField] private float interactDistance = 2f;

        [Header("Sanity")]
        [SerializeField] private float maxSanity = 100f;

        [SerializeField] private float decreaseSanityRate = 1f;
        [SerializeField] private float currentSanity;
        public float SanityPercent => currentSanity / maxSanity;

        [Header("Stamina")]
        [SerializeField] private float maxStamina = 100f;

        [SerializeField] private float decreaseStaminaRate = 1f;

        [SerializeField] private float rechargeStaminaRate = 2f;
        [SerializeField] private float rechargeStaminaDelay = 1f;

        [SerializeField] private float currentStamina;
        [SerializeField] private float currentStaminaDelayCounter;

        [SerializeField] private bool HasStamina = true;
        public float StaminaPercent => currentStamina / maxStamina;

        public delegate void CharacterStaminaAction(bool HasStamina);

        public static event CharacterStaminaAction OnExhausted;

        public delegate void CharacterNeedsAction(float Hunger, float Thirst, float Stamina);

        public static event CharacterNeedsAction OnNeedsChanged;

        [Header("Player Refernces")]
        [SerializeField] private Transform playerCameraTransform;
        private StarterAssetsInputs playerInputs;

        private void Start()
        {
            currentThirst = maxThirst;
            currentHunger = maxHunger;
            currentStamina = maxStamina;
            
            playerInputs = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            HandleStarvingAndThirst();
            HandleStamina();
            OnNeedsChanged?.Invoke(HungerPercent, ThirstPercent, StaminaPercent);
        }

        public void HandleEatingOrDrinking(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled)
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
            if (playerInputs.IsSprinting() && playerInputs.GetMove().y > 0.1f)
            {
                currentStamina -= decreaseStaminaRate * Time.deltaTime;

                HasStamina = currentStamina > 0;

                if (!HasStamina)
                {
                    OnExhausted(HasStamina);
                    currentStamina = 0;
                }
            }
            else if (currentStamina < maxStamina)
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

            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
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