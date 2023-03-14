using UnityEngine;
using UnityEngine.UI;

namespace OM
{
    public class HealthbarUI : MonoBehaviour
    {
        Image healthBar;

        private void Awake()
        {
            healthBar = GetComponent<Image>();
        }

        void OnEnable()
        {
            HealthSystem.OnHealthChanged += SetHealthbar;
        }

        void OnDisable()
        {
            HealthSystem.OnHealthChanged -= SetHealthbar;
        }

        void SetMaxHealthbar(float maxHealth)
        {
            healthBar.fillAmount = maxHealth;
        }

        public void SetHealthbar(float maxHealth, float currentHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}