using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

namespace OM
{
    public class TaskView : MonoBehaviour
    {
        [SerializeField] private GameObject taskPrefab;
        [SerializeField] private Transform taskContent;
        [SerializeField] private GameObject taskWindow;

        [FormerlySerializedAs("currentTasks")] public List<Task> allTasks;

        private void OnEnable()
        {
            PlayerHealth.OnPlayerDead += CloseTasksList;
        }
        private void OnDisable()
        {
            PlayerHealth.OnPlayerDead -= CloseTasksList;
        }
        
        private void Start()
        {
            InitializeTasks();
        }

        private void InitializeTasks()
        {
            foreach (var task in allTasks)
            {
                task.Initialize();
                task.taskCompleted.AddListener(OnTaskCompleted);

                GameObject taskObject = Instantiate(taskPrefab, taskContent);
                taskObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = task.information.name;


                taskObject.GetComponent<Button>().onClick.AddListener(delegate
                {
                    taskWindow.GetComponent<TaskWindowBuilder>().Initialize(task);
                    taskWindow.SetActive(true);
                    gameObject.SetActive(false);
                });
            }
        }



        private void OnTaskCompleted(Task task)
        {
            //Sets Checkmark to true
            //taskContent.GetChild(currentTasks.IndexOf(task)).Find("Done").gameObject.SetActive(true);
            Destroy(taskContent.GetChild(allTasks.IndexOf(task)).gameObject);
            allTasks.Remove(task);
        }

        public void CloseTasksList()
        {
            gameObject.SetActive(false);
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}