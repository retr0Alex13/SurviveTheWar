using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OM
{
    public class SanityBarUI : MonoBehaviour
    {
        Image sanityBar;
        private float sanityChangeSpeed = 0.5f;
        private float targetSanity;

        private void Awake()
        {
            sanityBar = GetComponent<Image>();
        }
        
        void OnEnable()
        {
            PlayerNeeds.OnSanityChanged += SetSanityBar;
        }

        void OnDisable()
        {
            PlayerNeeds.OnSanityChanged -= SetSanityBar;
        }

        public void SetSanityBar(float maxSanity, float currentSanity)
        {
            targetSanity = currentSanity / maxSanity;
            StartCoroutine(ChangeSanityBarSmoothly());
        }
        
        IEnumerator ChangeSanityBarSmoothly()
        {
            float elapsedTime = 0;
            float startingSanity = sanityBar.fillAmount;

            while (elapsedTime < sanityChangeSpeed)
            {
                elapsedTime += Time.deltaTime;
                sanityBar.fillAmount = Mathf.Lerp(startingSanity, targetSanity, elapsedTime / sanityChangeSpeed);
                yield return null;
            }

            sanityBar.fillAmount = targetSanity;
        }
    }
}