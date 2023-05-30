using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace OM
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public Sound[] sounds;
        public AudioMixerGroup audioMixer;
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
                sound.audioSource = gameObject.AddComponent<AudioSource>();
                sound.audioSource.outputAudioMixerGroup = audioMixer;
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
        

        public AudioClip GetAudioClip(string soundName)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.name == soundName)
                {
                    return sound.clips[0];
                }
            }
            Debug.LogError("Sound with " + soundName + " not found!");
            return null;
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

            GameObject soundGameObject = new GameObject(sound.name);
            soundGameObject.transform.position = position;
            soundGameObject.transform.parent = transform;
            
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clips[Random.Range(0, sound.clips.Length)];
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.volume = sound.volume;
            audioSource.loop = sound.isLoop;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 1f;
            audioSource.minDistance = sound.minDistance;
            audioSource.maxDistance = sound.maxDistance;
            audioSource.Play();
            
            if(audioSource.loop)
                return;
            GameObject.Destroy(soundGameObject, audioSource.clip.length);
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
            
            AudioClip randomClip = sound.clips[Random.Range(0, sound.clips.Length)];
            
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.isLoop;
            sound.audioSource.pitch = Random.Range(0.9f, 1.1f);
            sound.audioSource.PlayOneShot(randomClip);
        }

        public void StopSound(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);

            if (sound == null)
            {
                Debug.Log("Sound " + soundName + " Not Found!");
                return;
            }
            
            foreach (Transform child in transform)
            {
                if (child.name == sound.name)
                {
                    child.GetComponent<AudioSource>().Stop();
                    Destroy(child.gameObject);
                }
            }
            
            sound.audioSource.Stop();
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
    }
}