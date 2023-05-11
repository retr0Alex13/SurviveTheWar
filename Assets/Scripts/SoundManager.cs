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
                InitializeAudioSource(sound);
                InitializeSoundCooldown(sound);
            }
        }

        private static void InitializeSoundCooldown(Sound sound)
        {
            if (sound.hasCooldown)
            {
                soundTimerDictionary[sound.name] = 0f;
            }
        }

        private void InitializeAudioSource(Sound sound)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clips[Random.Range(0, sound.clips.Length)];

            sound.source.volume = sound.volume;
            // sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;
        }

        public AudioSource GetAudioClip(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);
            return sound?.source;

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

            AudioSource audioSource = CreateAudioSource("Sound", transform);
            audioSource.clip = sound.clips[Random.Range(0, sound.clips.Length)];
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
            sound.source.pitch = Random.Range(0.9f, 1.1f);

            AudioClip randomClip = sound.clips[Random.Range(0, sound.clips.Length)];
            sound.source.PlayOneShot(randomClip);
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
                float cooldownDuration = sound.cooldown > 0 && sound.hasCooldown ? sound.cooldown : GetLongestClipDuration(sound.clips);

                if (lastTimePlayed + cooldownDuration < Time.time)
                {
                    soundTimerDictionary[sound.name] = Time.time;
                    return true;
                }
                return false;
            }

            return true;
        }
        
        private static float GetLongestClipDuration(AudioClip[] clips)
        {
            float longestDuration = 0f;

            foreach (AudioClip clip in clips)
            {
                if (clip.length > longestDuration)
                {
                    longestDuration = clip.length;
                }
            }

            return longestDuration;
        }
        
        private static AudioSource CreateAudioSource(string name, Transform parent)
        {
            GameObject soundGameObject = new GameObject(name);
            soundGameObject.transform.SetParent(parent);
            return soundGameObject.AddComponent<AudioSource>();
        }
    }
}