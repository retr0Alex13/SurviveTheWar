using StarterAssets;
using UnityEngine;

public class CharacterNeeds : MonoBehaviour
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

    [Header("Stamina")]
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float decreaseStaminaRate = 1f;
    [SerializeField] float rechargeStaminaRate = 2f;
    [SerializeField] float rechargeStaminaDelay = 1f;
    [SerializeField] float currentStamina;
    [SerializeField] float currentStaminaDelayCounter;
    [SerializeField] bool HasStamina = true;
    public float StaminaPercent => currentStamina / maxStamina;

    public delegate void CharacterStaminaAction(bool HasStamina);
    public static event CharacterStaminaAction OnExhausted;

    public delegate void CharacterNeedsAction(float Hunger, float Thirst, float Stamina);
    public static event CharacterNeedsAction OnNeedsChanged;

    [Header("Player Refernces")]
    [SerializeField] private Transform playerCameraTransform;
    StarterAssetsInputs playerInputs;

    [SerializeField] private float coolDownTimer;

    private float coolDown = 0.5f;

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
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (coolDownTimer < 0)
        {
            coolDownTimer = 0;
        }
        HandleStarvingAndThirst();
        HandleStamina();
        HandleEatingOrDrinking();
        OnNeedsChanged(HungerPercent, ThirstPercent, StaminaPercent);
    }

    private void HandleEatingOrDrinking()
    {
        if (playerInputs.IsInteracting() && coolDownTimer == 0)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, interactDistance))
            {
                if (raycastHit.transform.TryGetComponent(out ItemEatable itemEatable))
                {
                    AddHungerAndThirst(itemEatable.FoodToRestore, itemEatable.ThirstToRestore);
                    Destroy(itemEatable.gameObject);
                }
            }
            coolDownTimer = coolDown;
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
        if (playerInputs.IsSprinting())
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

        if (!playerInputs.IsSprinting() && currentStamina < maxStamina)
        {
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

    void AddHungerAndThirst(float hungerAmount, float thirstAmount)
    {
        currentHunger += hungerAmount;
        currentThirst += thirstAmount;

        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
    }

}
