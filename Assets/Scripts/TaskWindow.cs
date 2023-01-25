using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform goalContent;

    public void Initialize(Task task)
    {
        titleText.text = task.information.name;
        descriptionText.text = task.information.description;
       

        foreach (var goal in task.goals)
        {
            GameObject goalObject = Instantiate(goalPrefab, goalContent);
            goalObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = goal.GetDescription();

            GameObject countObj = goalObject.transform.Find("Count").gameObject;
            
            if(goal.Completed)
            {
                countObj.SetActive(false);
            }
            else
            {
                countObj.GetComponent<TextMeshProUGUI>().text = goal.CurrentAmount + "/" + goal.requiredAmount;
            }
        }
    }
    public void CloseWindow()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < goalContent.childCount; i++)
        {
            Destroy(goalContent.GetChild(i).gameObject);
        }
    }
}
