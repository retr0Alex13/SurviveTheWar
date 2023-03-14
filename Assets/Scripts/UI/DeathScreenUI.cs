using System.Collections;
using UnityEngine;

namespace OM
{
    public class DeathScreenUI : MonoBehaviour
    {
        private CanvasGroup deathGroupUI;
        [SerializeField] private float alphaLerp = 1f;

        private void OnEnable()
        {
            GameManager.OnPlayerDead += SetVisibleDeathGroupUI;
        }

        private void OnDisable()
        {
            GameManager.OnPlayerDead -= SetVisibleDeathGroupUI;
        }

        private void Start()
        {
            deathGroupUI = gameObject.GetComponent<CanvasGroup>();
            deathGroupUI.blocksRaycasts = false;
        }
        private void SetVisibleDeathGroupUI()
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
                elapsedTime += Time.deltaTime * alphaLerp;
                yield return null;
            }
            deathGroupUI.alpha = targetAlpha;
            deathGroupUI.blocksRaycasts = true;
        }
    }
}