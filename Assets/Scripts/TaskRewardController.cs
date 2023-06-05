using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public class TaskRewardController : MonoBehaviour
    {
        [SerializeField] private Transform rewardSpawnPoint;
        [SerializeField] private List<ItemSO> rewardItems;

        public void SpawnRandomReward(int itemsAmount)
        {
            for (int i = 0; i < itemsAmount; i++)
            {
                int randomItemIndex = Random.Range(0, rewardItems.Count);
                Instantiate(rewardItems[randomItemIndex].Prefab, 
                    new Vector3(rewardSpawnPoint.position.x, rewardSpawnPoint.position.y, rewardSpawnPoint.position.z), 
                    Quaternion.identity);
            }

            if (itemsAmount != 0)
            {
                SoundManager.Instance.PlaySound("DoorBell", rewardSpawnPoint.position);
            }
        }
    }
}
