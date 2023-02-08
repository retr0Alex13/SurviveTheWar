using OM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    //public float minTimeBetweenEvent = 5f;
    //public float maxTimeBetweenEvent = 10f;
    [Range(0,1)]public float probabilityOfEvent = 0.1f;
    [SerializeField] private bool isEventActive = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    //[SerializeField] private float timeUntilNextEvent;


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
        //timeUntilNextEvent = Random.Range(minTimeBetweenEvent, maxTimeBetweenEvent);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    private void Update()
    {
        //HandleEvent();
        if (isEventActive && !audioSource.isPlaying)
        {
            isEventActive = false;
        }
    }

    private void HandleEvent()
    {
        if (!isEventActive)
        {
            //timeUntilNextEvent -= Time.deltaTime / 20f;
            //if (timeUntilNextEvent <= 0f)
            //{
                if (Random.value < probabilityOfEvent)
                {
                    TriggerEvent();
                }
                //timeUntilNextEvent = Random.Range(minTimeBetweenEvent, maxTimeBetweenEvent);
            //}
        }
    }

    private void TriggerEvent()
    {
        if (!audioSource.isPlaying)
        {
            isEventActive = true;
            audioSource.Play();
        }
        Debug.Log("Air Raid Alert!");
    }
}
