using OM;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DaySurvivedUI : MonoBehaviour
{
    private CanvasGroup daySurvivedGroupUI;
    [SerializeField] private DayNightSystem dayNightSystem;
    [SerializeField] private TextMeshProUGUI daysSurvivedText;
    [SerializeField] private float secondsToShowScreen = 2f;
    [SerializeField, Tooltip("How smooth screen wiil fade in/out")]
    private float alphaLerp = 0.5f;

    private void OnEnable()
    {
        DayNightSystem.OnNewDay += SetTextDaysSurvived;
        DayNightSystem.OnNewDay += HandleDaySurvivedScreenVisibility;
    }

    private void OnDisable()
    {
        DayNightSystem.OnNewDay -= SetTextDaysSurvived;
        DayNightSystem.OnNewDay -= HandleDaySurvivedScreenVisibility;
    }

    private void Start()
    {
        daySurvivedGroupUI = GetComponent<CanvasGroup>();
    }

    private void SetTextDaysSurvived()
    {
        daysSurvivedText.text = dayNightSystem.DayCount + " days";
    }

    private void HandleDaySurvivedScreenVisibility()
    {
        StartCoroutine(AnimateDaySurvivedScreen());
    }

    private IEnumerator AnimateDaySurvivedScreen()
    {
        float targetAlpha = 1;
        float currentAlpha = daySurvivedGroupUI.alpha;

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            daySurvivedGroupUI.alpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime);
            elapsedTime += Time.deltaTime * alphaLerp;
            yield return null;
        }
        daySurvivedGroupUI.alpha = targetAlpha;

        yield return new WaitForSeconds(secondsToShowScreen);

        targetAlpha = 0;
        currentAlpha = daySurvivedGroupUI.alpha;
        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            daySurvivedGroupUI.alpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime);
            elapsedTime += Time.deltaTime * alphaLerp;
            yield return null;
        }
        daySurvivedGroupUI.alpha = targetAlpha;
    }
}
