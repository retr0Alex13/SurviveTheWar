using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private DayNightPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [Tooltip("How long Day/Night wil be")]
    [SerializeField] private float timerModifier = 20;

    private void Start()
    {
        TimeOfDay = 13f;
    }


    private void Update()
    {
        if (Preset == null)
            return;

        //(Replace with a reference to the game time)
        TimeOfDay += Time.deltaTime / timerModifier;
        TimeOfDay %= 24; //Modulus to ensure always between 0-24
        UpdateLighting(TimeOfDay / 24f);
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

        Debug.Log(GetHour());

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
        return Mathf.FloorToInt(TimeOfDay);
    }

    public bool IsDayTime()
    {
        return GetHour() > 6 && GetHour() < 18;
    }
}
