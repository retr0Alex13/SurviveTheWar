using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Task : ScriptableObject
{
    [Serializable]
    public struct Info
    {
        public string name;
        public string description;
    }

    [Header("Info")] public Info information;

    [Serializable]
    public struct Reward
    {
        public int money;
        public int sanity;
    }

    [Header("Reward")] public Reward reward = new Reward { money = 10, sanity = 10 };

    public bool Completed { get; protected set; }
    public TaskCompletedEvent taskCompleted;

    public abstract class TaskGoal : ScriptableObject
    {
        protected string description;
        public int CurrentAmount { get; protected set; }
        public int requiredAmount = 1;

        public bool Completed { get; protected set; }
        [HideInInspector] public UnityEvent goalCompleted;

        public virtual string GetDescription()
        {
            return description;
        }

        public virtual void Initialize()
        {
            Completed = false;
            goalCompleted = new UnityEvent();
        }

        protected void Evaluate()
        {
            if (CurrentAmount >= requiredAmount)
            {
                Complete();
            }
        }

        private void Complete()
        {
            Completed = true;
            goalCompleted.Invoke();
            goalCompleted.RemoveAllListeners();
        }
    }

    public List<TaskGoal> goals;

    public void Initialize()
    {
        Completed = false;
        taskCompleted = new TaskCompletedEvent();

        foreach (var goal in goals)
        {
            goal.Initialize();
            goal.goalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }

    private void CheckGoals()
    {
        Completed = goals.All(g => g.Completed);
        if (Completed)
        {
            //Give reward
            taskCompleted.Invoke(this);
            taskCompleted.RemoveAllListeners();
        }
    }
}

public class TaskCompletedEvent : UnityEvent<Task> { }

#if UNITY_EDITOR
[CustomEditor(typeof(Task))]
public class TaskEditor : Editor
{

    SerializedProperty m_TaskInfoProperty;
    SerializedProperty m_TaskRewardProperty;

    List<string> m_TaskGoalType;
    SerializedProperty m_TaskGoalListProperty;

    [MenuItem("Assets/New Task", priority = 0)]
    public static void CreateTask()
    {
        var newTask = CreateInstance<Task>();

        ProjectWindowUtil.CreateAsset(newTask, "task.asset");
    }

    private void OnEnable()
    {
        m_TaskInfoProperty = serializedObject.FindProperty(nameof(Task.information));
        m_TaskRewardProperty = serializedObject.FindProperty(nameof(Task.reward));

        m_TaskGoalListProperty = serializedObject.FindProperty(nameof(Task.goals));

        var lookup = typeof(Task.TaskGoal);
        m_TaskGoalType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        var child = m_TaskInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Task info", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        child = m_TaskRewardProperty.Copy();
        depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Task reward", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }



        int choice = EditorGUILayout.Popup("Add new task goal", -1, m_TaskGoalType.ToArray());

        if (choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_TaskGoalType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_TaskGoalListProperty.InsertArrayElementAtIndex(m_TaskGoalListProperty.arraySize);
            m_TaskGoalListProperty.GetArrayElementAtIndex(m_TaskGoalListProperty.arraySize - 1)
                .objectReferenceValue = newInstance;
        }

        Editor ed = null;
        int toDelete = -1;
        for (int i = 0; i < m_TaskGoalListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_TaskGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (toDelete != -1)
        {
            var item = m_TaskGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            m_TaskGoalListProperty.DeleteArrayElementAtIndex(toDelete);
            m_TaskGoalListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif