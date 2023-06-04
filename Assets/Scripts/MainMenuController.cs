using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OM
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject cinemachineVirtualCamera;
        [SerializeField] private GameObject OptionsMenu;
        [SerializeField] private CanvasGroup fadeInCanvasGroup;
        [SerializeField] private float alphaLerp = 0.5f;
        [SerializeField] private PowerGeneratorController generatorController;
        [SerializeField] private DayNightCycle dayNightCycle;

        private void Update()
        {
            GeneratorPower();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        private void GeneratorPower()
        {
            if (dayNightCycle.IsNight())
            {
                if (generatorController.isTurnedOn)
                    return;
                generatorController.Interact();
            }
            else
            {
                if (!generatorController.isTurnedOn)
                    return;
                generatorController.Interact();
            }
        }

        public void StartGame()
        {
            StartCoroutine(FadeInAndStartGame());
        }

        private IEnumerator FadeInAndStartGame()
        {
            float targetAlpha = 1;
            float currentAlpha = fadeInCanvasGroup.alpha;

            float elapsedTime = 0;
            while (elapsedTime < 1)
            {
                fadeInCanvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime);
                elapsedTime += Time.deltaTime * alphaLerp;
                yield return null;
            }
            fadeInCanvasGroup.alpha = targetAlpha;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ShowOptions()
        {
            cinemachineVirtualCamera.SetActive(true);
            OptionsMenu.SetActive(true);
            
        }

        public void ExitOptions()
        {
            cinemachineVirtualCamera.SetActive(false);
            OptionsMenu.SetActive(false);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
