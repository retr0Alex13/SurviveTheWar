using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace OM
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip[] clips;

        [Range(0f, 1f)]
        public float volume = 1f;

        //[Range(.1f, 3f)]
        //public float pitch = 1f;

        public bool isLoop;
        public bool hasCooldown;
        public float cooldown;
        public float minDistance = 1.5f;
        public float maxDistance = 500;
        public AudioSource audioSource;
    }
}