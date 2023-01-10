using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNeedsUI : MonoBehaviour
{
    [SerializeField] Image hungerMeter, thirstMeter, staminaBar;
    void OnEnable()
    {
        CharacterNeeds.OnNeedsChanged += ChangeCharacterNeedsUI;
    }

    void OnDisable()
    {
        CharacterNeeds.OnNeedsChanged -= ChangeCharacterNeedsUI;
    }

    private void ChangeCharacterNeedsUI(float hungerPercent, float thirstPercent, float staminaPercent)
    {
        hungerMeter.fillAmount = hungerPercent;
        thirstMeter.fillAmount = thirstPercent;
        staminaBar.fillAmount = staminaPercent;
    }
}
