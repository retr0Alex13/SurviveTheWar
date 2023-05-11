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
        [SerializeField] private float currentTimeOfEvent;
        [SerializeField] private float eventDuration;


        [SerializeField] private float timeToGetToSafe = 10f;
        [SerializeField] private float currentTimeToGetSafe;

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
            playerHealth = FindAnyObjectByType<PlayerHealth>();
            eventDuration = SoundManager.Instance.GetAudioClip("Air Raid Alert").clip.length;
        }
        private void Update()
        {
            if (isEventActive)
            {
                StartEventTimer();
            }
        }

        private void StartEventTimer()
        {
            currentTimeToGetSafe -= Time.deltaTime;
            currentTimeOfEvent -= Time.deltaTime;
            if (currentTimeToGetSafe <= 0)
            {
                currentTimeToGetSafe = 0;
                if (!isInSafeZone)
                {
                    playerHealth.playerHealth.DamageEntity(eventDamage);
                }
            }
            if(currentTimeOfEvent <= 0)
            {
                isEventActive = false;
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
            isEventActive = true;
            currentTimeToGetSafe = timeToGetToSafe;
            currentTimeOfEvent = eventDuration;
            SoundManager.Instance.PlaySound("Air Raid Alert");
            Debug.Log("Air Raid Alert!");
        }
    }
}