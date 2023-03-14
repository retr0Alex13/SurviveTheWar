using UnityEngine;

namespace OM
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Daynight Preset", menuName = "ScriptableObjects/Lighting Preset", order = 1)]
    public class DayNightPreset : ScriptableObject
    {
        public Gradient AmbientColor;
        public Gradient DirectionalColor;
        public Gradient FogColor;
    }
}