using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace OM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager { get; private set; }
        public TaskRewardController taskRewardController;

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

            taskRewardController = GetComponent<TaskRewardController>();
        }
    }
}