using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class QuestManager
{
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestManager();
            }
            return instance;
        }
    }

    [System.Serializable]
    public class QuestTask
    {
        public QuestData_So questData;

        public bool isStarted;
        public bool isFinished;
        public bool isComplete;
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    public QuestManager()
    {
        tasks = JsonMgr.Instance.LoadData<List<QuestTask>>("TaskData");
        foreach (QuestTask task in tasks)
        {
            task.questData.isStarted = task.isStarted;
            task.questData.isComplete = task.isComplete;
            task.questData.isFinished = task.isFinished;
        }
    }

    public void SaveTaskData()
    {
        JsonMgr.Instance.SaveData(tasks, "TaskData");
    }

    //敌人死亡，收集物品
    public void UpdateQuestProgress(string requirementName, int amount)
    {
        foreach (var task in tasks)
        {
            if (task.isFinished)
                continue;

            var targetTask = task.questData.questRequires.Find(r => r.name == requirementName);
            if(targetTask != null)
            {
                targetTask.currentAmount += amount;
                if (targetTask.currentAmount >= targetTask.requireAmount)
                    targetTask.currentAmount = targetTask.requireAmount;
            }
            //检查任务是否完成
            task.isComplete = task.questData.CheckQuestProgress();
        }
    }

    public bool HaveQuest(QuestData_So data)
    {
        if (data != null)
            return tasks.Any(q => q.questData.questName == data.questName);
        else
            return false;
    }

    public QuestTask GetTask(QuestData_So data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }

    public void NewQuestData()
    {
        tasks.Clear();
    }
}
