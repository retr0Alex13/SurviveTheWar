using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace OM
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        private Resolution[] resolutions;

        public TMP_Dropdown resolutionDropDown;

        private void Start()
        {
            LoadSettings();
            HandleResolutionSetting();
        }

        private void LoadSettings()
        {
            if (PlayerPrefs.HasKey("volume"))
            {
                float volume = PlayerPrefs.GetFloat("volume");
                SetVolume(volume);
            }

            if (PlayerPrefs.HasKey("quality"))
            {
                int quality = PlayerPrefs.GetInt("quality");
                SetQuality(quality);
            }
            
            if (PlayerPrefs.HasKey("msaa"))
            {
                int mSAA = PlayerPrefs.GetInt("msaa");
                switch (mSAA)
                {
                    case 0:
                        QualitySettings.antiAliasing = 2;
                        break;
                    case 1:
                        QualitySettings.antiAliasing = 4;
                        break;
                    case 2:
                        QualitySettings.antiAliasing = 8;
                        break;
                    default:
                        QualitySettings.antiAliasing = 8;
                        break;
                }
                PlayerPrefs.SetInt("msaa", mSAA);
                SetMSAA(mSAA);
            }

            if (PlayerPrefs.HasKey("fullscreen"))
            {
                int fullscreen = PlayerPrefs.GetInt("fullscreen");
                SetFullScreen(fullscreen == 1);
            }

            if (PlayerPrefs.HasKey("vsync"))
            {
                int vSync = PlayerPrefs.GetInt("vsync");
                SetVsync(vSync == 1);
            }
            else
            {
                SetVsync(true);
            }
        }

        public void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        private void HandleResolutionSetting()
        {
            resolutions = Screen.resolutions;
            resolutionDropDown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropDown.AddOptions(options);
            resolutionDropDown.value = currentResolutionIndex;
            resolutionDropDown.RefreshShownValue();
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
            PlayerPrefs.SetFloat("volume", volume);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt("quality", qualityIndex);
        }

        public void SetResolution(int resolutionScreenIndex)
        {
            Resolution resolution = resolutions[resolutionScreenIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
        }

        public void SetVsync(bool vSyncIndex)
        {
            QualitySettings.vSyncCount = vSyncIndex ? 1 : 0;
            PlayerPrefs.SetInt("vsync", vSyncIndex ? 1 : 0);
        }

        public void SetMSAA(int mSAAindex)
        {
            switch (mSAAindex)
            {
                case 0:
                    QualitySettings.antiAliasing = 2;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 8;
                    break;
                default:
                    QualitySettings.antiAliasing = 8;
                    break;
            }
            PlayerPrefs.SetInt("msaa", mSAAindex);
        }
    }
}
