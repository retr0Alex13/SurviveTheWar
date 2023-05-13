using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class PlayPageSound : MonoBehaviour
    {
        public void PlayTurnPageSound()
        {
            SoundManager.Instance.PlaySound("TurnPageOver");
        }
    }
}
