using System.Collections;
using UnityEngine;

namespace OM
{
    public class DeathScreenView : MonoBehaviour
    {
        private CanvasGroup deathGroupUI;
        [SerializeField] private float alphaLerp = 1f;

        // private void OnEnable()
        // {
        //     PlayerHealth.OnPlayerDead += SetVisibleDeathGroupUI;
        // }
        //
        // private void OnDisable()
        // {
        //     PlayerHealth.OnPlayerDead -= SetVisibleDeathGroupUI;
        // }

        private void Start()
        {
            deathGroupUI = gameObject.GetComponent<CanvasGroup>();
            deathGroupUI.blocksRaycasts = false;
        }
        public void SetVisibleDeathGroupUI()
        {
            StartCoroutine(AnimateDeathScreen());
        }

        private IEnumerator AnimateDeathScreen()
        {
            float targetAlpha = 1;
            float currentAlpha = deathGroupUI.alpha;

            float elapsedTime = 0;
            while (elapsedTime < 1)
            {
                deathGroupUI.alpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime);
                elapsedTime += Time.unscaledDeltaTime * alphaLerp;
                yield return null;
            }
            deathGroupUI.alpha = targetAlpha;
            deathGroupUI.blocksRaycasts = true;
        }
    }
}