using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OM
{
    public class HealthbarUI : MonoBehaviour
    {
        Image healthBar;
        [SerializeField] private float healthChangeSpeed = 0.5f;
        private float targetHealth;

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
            //healthBar.fillAmount = currentHealth / maxHealth;
            targetHealth = currentHealth / maxHealth;
            StartCoroutine(ChangeHealthbarSmoothly());
        }
        
        IEnumerator ChangeHealthbarSmoothly()
        {
            float elapsedTime = 0;
            float startingHealth = healthBar.fillAmount;

            while (elapsedTime < healthChangeSpeed)
            {
                elapsedTime += Time.deltaTime;
                healthBar.fillAmount = Mathf.Lerp(startingHealth, targetHealth, elapsedTime / healthChangeSpeed);
                yield return null;
            }

            healthBar.fillAmount = targetHealth;
        }
    }
}