using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager { get; private set; }
        public HealthSystem playerHealth = new HealthSystem(100, 100);
        private int money;
        public int Money { get { return money; } }

        //Singleton
        void Awake()
        {
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