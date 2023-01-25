using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private GameObject taskPrefab;
    [SerializeField] private Transform taskContent;
    [SerializeField] private GameObject taskWindow;

    public List<Task> currentTasks;

    private void Start()
    {
        foreach (var task in currentTasks)
        {
            task.Initialize();
            task.taskCompleted.AddListener(OnTaskCompleted);

            GameObject taskObject = Instantiate(taskPrefab, taskContent);                

            taskObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                taskWindow.GetComponent<TaskWindow>().Initialize(task);
                taskWindow.SetActive(true);
            });
        }
    }

    public void Gather(string itemName)
    {
        EventManager.Instance.QueueEvent(new GatheringGameEvent(itemName));
    }

    private void OnTaskCompleted(Task task)
    {
        //Sets Checkmark to true
        //taskContent.GetChild(currentTasks.IndexOf(task)).Find("Checkmark").gameObjectSetActive(true);
    }
}
