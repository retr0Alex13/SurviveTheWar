using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager { get; private set; }

        [Header("Player References")]
        [SerializeField] private PlayerInput playerInput;

        public HealthSystem playerHealth = new HealthSystem(100, 100);

        public delegate void GameManagerAction();
        public static event GameManagerAction OnPlayerDead;

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

        private void Update()
        {
            if (playerHealth.CurrentHealth <= 0)
            {
                OnPlayerDead();
                FreezePlayer();
            }
        }

        private void FreezePlayer()
        {
            playerInput.enabled = false;
        }
    }
}