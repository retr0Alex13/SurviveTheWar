using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
            taskObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = task.information.name;
            

            taskObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                taskWindow.GetComponent<TaskWindow>().Initialize(task);
                taskWindow.SetActive(true);
                gameObject.SetActive(false);
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
        //taskContent.GetChild(currentTasks.IndexOf(task)).Find("Done").gameObject.SetActive(true);
        Destroy(taskContent.GetChild(currentTasks.IndexOf(task)).gameObject);
        currentTasks.Remove(task);
    }
}
