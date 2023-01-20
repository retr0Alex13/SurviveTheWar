using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    //Scene References
    [Header("Lights")]
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private DayNightPreset Preset;
    //Variables
    [Header("Day cycle settings")]
    [SerializeField] private int dayCount = 0;
    [SerializeField, Tooltip("Current time of day"), Range(0, 24)]
    private float timeOfDay;
    [SerializeField, Tooltip("At what o'clock player will start day")] 
    private float startDayHour = 9;
    [SerializeField, Tooltip("At what o'clock player will end day")] 
    private float endDayHour = 21;
    [Tooltip("How long Day/Night wil be")]
    [SerializeField] private float dayNightTimerModifier = 20;
    [SerializeField] private float fogDencity = 0.05f;
    private float lightIntencityLerp = 1f;
    private float fogIntencityLerp = 0.5f;

    private void Start()
    {
        timeOfDay = startDayHour;
    }


    private void Update()
    {
        HandleDayCycle();
    }

    private void SetDayHour(float startDayHour)
    {
        timeOfDay = startDayHour;
    }    

    private void NextDay()
    {
        dayCount++;
        SetDayHour(startDayHour);
    }

    private void HandleDayCycle()
    {
        if (Preset == null)
            return;
        if (timeOfDay >= endDayHour)
        {
            NextDay();
        }
        timeOfDay += Time.deltaTime / dayNightTimerModifier;
        timeOfDay %= 24; //Modulus to ensure always between 0-24
        UpdateLighting(timeOfDay / 24f);
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

        if (!IsDayTime())
        {
            DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 0, Time.deltaTime * lightIntencityLerp);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDencity, Time.deltaTime * fogIntencityLerp);
        }
        else
        {
            DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 1f, Time.deltaTime * lightIntencityLerp);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0, Time.deltaTime * fogIntencityLerp);
        }

        //Debug.Log(GetHour());

    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    public int GetHour()
    {
        return Mathf.FloorToInt(timeOfDay);
    }

    public bool IsDayTime()
    {
        return GetHour() > 6 && GetHour() < 19;
    }
}
