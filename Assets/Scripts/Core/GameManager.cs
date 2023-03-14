using StarterAssets;
using UnityEngine;

namespace OM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager { get; private set; }
        [Header("Player References")]
        [SerializeField] private FirstPersonController playerController;
        public HealthSystem playerHealth = new HealthSystem(100, 100);

        public delegate void GameManagerAction();
        public static event GameManagerAction OnPlayerDead;

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
            playerController.enabled = false;
        }
    }
}