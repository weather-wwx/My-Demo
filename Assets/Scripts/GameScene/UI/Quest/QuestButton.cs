using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public Text txtName;

    public QuestData_So currentData;

    public Text questContentText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContentText.text = currentData.questDescription;
        UIManager.Instance.GetPanel<QuestPanel>("QuestPanel").SetupRequirList(currentData);
        UIManager.Instance.GetPanel<QuestPanel>("QuestPanel").SetupRewardItem(currentData);
    }

    public void SetTaskName(QuestManager.QuestTask task)
    {
        currentData = task.questData;

        if(task.isComplete)
        {
            txtName.text = task.questData.questName + "£¨Íê³É£©";
        }
        else
        {
            txtName.text = task.questData.questName;
        }
    }
}
