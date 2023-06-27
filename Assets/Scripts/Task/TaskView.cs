using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace OM
{
    public class TaskView : MonoBehaviour
    {
        [SerializeField] private GameObject taskPrefab;
        [SerializeField] private Transform taskContent;
        [SerializeField] private GameObject taskWindow;
        [SerializeField] private int tasksPerDay;

        [FormerlySerializedAs("currentTasks")] public List<Task> allTasks;
        List<Task> listOfRandomTasks = new List<Task>();

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
            // InitializeTasks();
            GenerateDailyTasks();
        }

        // private void InitializeTasks()
        // {
        //     foreach (var task in allTasks)
        //     {
        //         task.Initialize();
        //         task.taskCompleted.AddListener(OnTaskCompleted);
        //
        //         GameObject taskObject = Instantiate(taskPrefab, taskContent);
        //         taskObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = task.information.name;
        //
        //
        //         taskObject.GetComponent<Button>().onClick.AddListener(delegate
        //         {
        //             taskWindow.GetComponent<TaskWindowBuilder>().Initialize(task);
        //             taskWindow.SetActive(true);
        //             gameObject.SetActive(false);
        //         });
        //     }
        // }
        
        public void GenerateDailyTasks()
        {
            foreach (Task task in listOfRandomTasks.ToList())
            {
                Destroy(taskContent.GetChild(listOfRandomTasks.IndexOf(task)).gameObject);
                listOfRandomTasks.Remove(task);
            }

            foreach (Task task in listOfRandomTasks.ToList())
            {
                Debug.Log(task);
            }
            listOfRandomTasks.Clear();

            // Generate new random tasks for the day
            for (int i = 0; i < tasksPerDay; i++)
            {
                Task randomTask = GetRandomTaskFromList();
                listOfRandomTasks.Add(randomTask);
            }

            foreach (Task randomTask in listOfRandomTasks)
            {
                randomTask.Initialize();
                randomTask.taskCompleted.AddListener(OnTaskCompleted);

                // Instantiate and set up the UI for the task
                GameObject taskObject = Instantiate(taskPrefab, taskContent);
                taskObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = randomTask.information.name;

                taskObject.GetComponent<Button>().onClick.AddListener(delegate
                {
                    taskWindow.GetComponent<TaskWindowBuilder>().Initialize(randomTask);
                    taskWindow.SetActive(true);
                    //gameObject.SetActive(false);
                });
            }
        }
        
        private Task GetRandomTaskFromList()
        {
            int randomIndex = Random.Range(0, allTasks.Count);
            return allTasks[randomIndex];
        }
        

        private void OnTaskCompleted(Task task)
        {
            //Sets Checkmark to true
            //taskContent.GetChild(currentTasks.IndexOf(task)).Find("Done").gameObject.SetActive(true);
            SoundManager.Instance.PlaySound("TaskCompleted");
            Destroy(taskContent.GetChild(listOfRandomTasks.IndexOf(task)).gameObject);
            listOfRandomTasks.Remove(task);
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