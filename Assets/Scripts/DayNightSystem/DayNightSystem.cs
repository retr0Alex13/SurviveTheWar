using UnityEngine;

namespace OM
{
    public class DayNightSystem : MonoBehaviour
    {
        [Header("Lights")]
        [SerializeField] private Light DirectionalLight;
        [SerializeField] private DayNightPreset Preset;

        [Header("Day cycle settings")]
        [SerializeField] private int dayCount = 0;
        public int DayCount => dayCount;
        [SerializeField, Tooltip("Current time of day"), Range(0, 24)]
        private float timeOfDay;
        [SerializeField, Tooltip("At what o'clock player will start day")]
        private float startDayHour = 9;
        [SerializeField, Tooltip("At what o'clock player will end day")]
        private float endDayHour = 21;
        [Tooltip("How long Day/Night wil be")]
        [SerializeField] private float dayNightTimerModifier = 20;
        //[SerializeField] private float fogDencity = 0.05f;
        private float lightIntencityLerp = 1f;
        //private float fogIntencityLerp = 0.5f;

        public delegate void DayNightAction();
        public static event DayNightAction OnNewHour;
        public static event DayNightAction OnNewDay;

        private void Start()
        {
            timeOfDay = startDayHour;
        }


        private void Update()
        {
            HandleDayCycle();
        }

        private void SetDay(int day)
        {
            if (day < 0)
            {
                return;
            }
            dayCount = day;
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

            CheckForNextHour();

            if (timeOfDay >= endDayHour)
            {
                NextDay();
            }
            else if (timeOfDay >= endDayHour - 0.2f)
            {
                OnNewDay?.Invoke();
            }
            timeOfDay += Time.deltaTime / dayNightTimerModifier;
            timeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(timeOfDay / 24f);
        }

        private void CheckForNextHour()
        {
            int currentHour = GetHour();
            int nextHour = Mathf.FloorToInt((timeOfDay + Time.deltaTime / dayNightTimerModifier) % 24);
            if (currentHour != nextHour)
            {
                //New hour started
                OnNewHour?.Invoke();
            }
            else
            {
                //New hour not yet started
            }
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
                //RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDencity, Time.deltaTime * fogIntencityLerp);
            }
            else
            {
                DirectionalLight.intensity = Mathf.Lerp(DirectionalLight.intensity, 1f, Time.deltaTime * lightIntencityLerp);
                //RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0, Time.deltaTime * fogIntencityLerp);
            }
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
                Light[] lights = FindObjectsOfType<Light>();
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
}