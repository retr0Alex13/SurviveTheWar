using UnityEngine;

namespace OM
{
    public class EventSystem : MonoBehaviour
    {
        public bool isInSafeZone = false;

        [Header("Event")]
        [Range(0, 1), SerializeField] private float probabilityOfEvent = 0.1f;
        [SerializeField] private bool isEventActive = false;
        [SerializeField] private int eventDamage = 35;

        [Header("Timer")]
        [SerializeField] private float timeToGetToSafe = 10f;
        [SerializeField] private float currentTimeToGetSafe;

        [Header("Audio of Event")]
        private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        private PlayerHealth playerHealth;

        private void OnEnable()
        {
            DayNightSystem.OnNewHour += HandleEvent;
        }

        private void OnDisable()
        {
            DayNightSystem.OnNewHour -= HandleEvent;
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            playerHealth = (PlayerHealth)FindAnyObjectByType(typeof(PlayerHealth));
            audioSource.clip = audioClip;
        }
        private void Update()
        {
            if (isEventActive && !audioSource.isPlaying)
            {
                isEventActive = false;
            }
            if (isEventActive)
            {
                StartEventTimer();
            }
        }

        private void StartEventTimer()
        {
            currentTimeToGetSafe -= Time.deltaTime;
            if (currentTimeToGetSafe <= 0)
            {
                currentTimeToGetSafe = 0;
                if (!isInSafeZone)
                {
                    playerHealth.playerHealth.DamageEntity(eventDamage);
                }
            }
        }

        private void HandleEvent()
        {
            if (!isEventActive)
            {
                if (Random.value < probabilityOfEvent)
                {
                    TriggerEvent();
                }
            }
        }

        private void TriggerEvent()
        {
            if (!audioSource.isPlaying)
            {
                isEventActive = true;
                audioSource.Play();
            }
            currentTimeToGetSafe = timeToGetToSafe;
            Debug.Log("Air Raid Alert!");
        }
    }
}