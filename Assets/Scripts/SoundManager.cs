using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OM
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public Sound[] sounds;
        private static Dictionary<string, float> soundTimerDictionary;

        public static SoundManager Instance
        {
            get { return _instance; }
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

            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.isLoop;

                if (sound.hasCooldown)
                {
                    Debug.Log(sound.name);
                    soundTimerDictionary[sound.name] = 0f;
                }
            }
        }

        public AudioSource GetAudioClip(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);
            return sound.source;

        }
        public void PlaySound(string soundName, Vector3 position)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);

            if (sound == null)
            {
                Debug.LogError("Sound " + soundName + " Not Found!");
                return;
            }

            if (!CanPlaySound(sound)) return;

            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.SetParent(transform);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.isLoop;
            audioSource.Play();
        }

        public void PlaySound(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);

            if (sound == null)
            {
                Debug.LogError("Sound " + soundName + " Not Found!");
                return;
            }

            if (!CanPlaySound(sound)) return;
            sound.source.PlayOneShot(sound.clip);
        }

        public void StopSound(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);

            if (sound == null)
            {
                Debug.LogError("Sound " + soundName + " Not Found!");
                return;
            }

            sound.source.Stop();
        }

        private static bool CanPlaySound(Sound sound)
        {
            if (soundTimerDictionary.ContainsKey(sound.name))
            {
                float lastTimePlayed = soundTimerDictionary[sound.name];

                if (sound.cooldown > 0 && sound.hasCooldown)
                {
                    if (lastTimePlayed + sound.cooldown < Time.time)
                    {
                        soundTimerDictionary[sound.name] = Time.time;
                        return true;
                    }
                }
                else
                {
                    if (lastTimePlayed + sound.clip.length < Time.time)
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