using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueControl))]
public class QuestGiver : MonoBehaviour
{
    DialogueControl control;
    QuestData_So currentQuest;

    public DialogueData_So startDialogue;
    public DialogueData_So progressDialogue;
    public DialogueData_So completeDialogue;
    public DialogueData_So finishDialogue;

    #region 切换不同的任务状态
    public bool IsStarted
    {
        get
        {
            if(QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).isStarted;
            }
            else
                return false;
        }
    }

    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).isComplete;
            }
            else
                return false;
        }
    }

    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).isFinished;
            }
            else
                return false;
        }
    }
    #endregion

    void Awake()
    {
        control = GetComponent<DialogueControl>();
    }

    void Start()
    {
        control.currentData = startDialogue;
        currentQuest = control.currentData.GetQuest();
    }

    void Update()
    {
        if(IsStarted)
        {
            if(IsComplete)
            {
                control.currentData = completeDialogue;
            }
            else
            {
                control.currentData = progressDialogue;
            }
        }

        if(IsFinished)
        {
            control.currentData = finishDialogue;
        }
    }
}
