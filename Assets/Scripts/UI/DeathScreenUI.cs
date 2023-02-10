using OM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    private void SetVisibleDeathGroupUI()
    {
        deathGroupUI.alpha += Mathf.Lerp(deathGroupUI.alpha, 1, alphaLerp * Time.deltaTime);
    }

    private void SetUnvisibleDeathGroupUI()
    {
        deathGroupUI.alpha = 0;
    }
}
