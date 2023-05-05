using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager { get; private set; }

        private int money;
        public int Money { get { return money; } }

        void Awake()
        {
            //Singleton
            if (gameManager != null && gameManager != this)
            {
                Destroy(this);
            }
            else
            {
                gameManager = this;
            }
        }
    }
}