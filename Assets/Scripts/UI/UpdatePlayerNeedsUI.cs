using UnityEngine;
using UnityEngine.UI;

namespace OM
{
    public class UpdatePlayerNeedsUI : MonoBehaviour
    {
        [SerializeField] private Image hungerMeter, thirstMeter, staminaBar;
        void OnEnable()
        {
            PlayerNeeds.OnNeedsChanged += ChangeCharacterNeedsUI;
        }

        void OnDisable()
        {
            PlayerNeeds.OnNeedsChanged -= ChangeCharacterNeedsUI;
        }

        private void ChangeCharacterNeedsUI(float hungerPercent, float thirstPercent, float staminaPercent)
        {
            hungerMeter.fillAmount = hungerPercent;
            thirstMeter.fillAmount = thirstPercent;
            staminaBar.fillAmount = staminaPercent;
        }
    }
}