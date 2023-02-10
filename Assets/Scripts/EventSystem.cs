using OM;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    [Range(0, 1), SerializeField] private float probabilityOfEvent = 0.1f;
    [SerializeField] private float timeToGetToSafe = 10f;
    [SerializeField] private int damagePlayerByEvent = 50;
    [SerializeField] private float currentTimeToGetSafe;
    [SerializeField] private bool isEventActive = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool isInSafeZone = false;
    [SerializeField] private BoxCollider safeZoneCollider;

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
        audioSource.clip = audioClip;
    }

    private void Update()
    {
        if (isEventActive && !audioSource.isPlaying)
        {
            isEventActive = false;
        }
        if(isEventActive)
        {
            CheckIfInSafeZone();
            StartEventTimer();
        }
    }

    private void StartEventTimer()
    {
        currentTimeToGetSafe -= Time.deltaTime;
        if (currentTimeToGetSafe <= 0)
        {
            currentTimeToGetSafe = 0;
            if(!isInSafeZone)
            {
                GameManager.gameManager.playerHealth.DamageEntity(damagePlayerByEvent);
                //return;
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

    private void CheckIfInSafeZone()
    {
        Collider[] colliderArray = Physics.OverlapBox(safeZoneCollider.transform.position, safeZoneCollider.size,
            safeZoneCollider.transform.rotation);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out FirstPersonController player))
            {
                isInSafeZone = true;
            }
            else
            {
                isInSafeZone = false;
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
