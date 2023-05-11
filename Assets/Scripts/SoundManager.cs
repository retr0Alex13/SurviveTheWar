using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public Sound[] sounds;
        private static Dictionary<string, float> soundTimerDictionary;

        public static SoundManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            soundTimerDictionary = new Dictionary<string, float>();
        }

        public AudioClip GetAudioClip(string soundName)
        {
            Sound sound = GetSound(soundName);
            return sound.clip;
        }

        public Sound GetSound(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);
            return sound;
        }
        public void PlaySound(string soundName, Vector3 position)
        {
            Sound sound = GetSound(soundName);

            if (sound == null)
            {
                Debug.LogError("Sound " + soundName + " Not Found!");
                return;
            }

            if (!CanPlaySound(sound)) return;

            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(soundName);
            
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.isLoop;
            audioSource.spatialBlend = 1f;

            if (sound.hasCooldown)
            {
                Debug.Log(sound.name);
                soundTimerDictionary[sound.name] = 0f;
            }
            audioSource.Play();
        }
        
        public void PlaySound(string soundName)
        {
            Sound sound = GetSound(soundName);

            if (sound == null)
            {
                Debug.LogError("Sound " + soundName + " Not Found!");
                return;
            }

            if (!CanPlaySound(sound)) return;

            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.isLoop;

            if (sound.hasCooldown)
            {
                Debug.Log(sound.name);
                soundTimerDictionary[sound.name] = 0f;
            }
            audioSource.PlayOneShot(sound.clip);
        }

        public void StopSound(string soundName)
        {
            Sound sound = GetSound(soundName);

            if (sound == null)
            {
                Debug.LogError("Sound " + soundName + " Not Found!");
                return;
            }

            foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            {
                if (sound.clip == audioSource.clip)
                {
                    audioSource.Stop();
                }
            }
        }

        private static bool CanPlaySound(Sound sound)
        {
            if (soundTimerDictionary.ContainsKey(sound.name))
            {
                float lastTimePlayed = soundTimerDictionary[sound.name];

                if(sound.useDefaultCoolDown)
                {
                    if (lastTimePlayed + sound.clip.length < Time.time)
                    {
                        soundTimerDictionary[sound.name] = Time.time;
                        return true;
                    }
                }
                else
                {
                    if (lastTimePlayed + sound.coolDown < Time.time)
                    {
                        soundTimerDictionary[sound.name] = Time.time;
                        return true;
                    }
                }
                

                return false;
            }

            return true;
        }
    }
}