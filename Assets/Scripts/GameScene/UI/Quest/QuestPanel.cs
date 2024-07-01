using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : BasePanel
{
    private Transform content;
    public Transform requireTransform;
    public Transform rewardTransform;

    public GameObject requirement;
    public GameObject rewardItem;
    public GameObject btnQuest;

    protected override void Awake()
    {
        base.Awake();
        content = GetControl<ScrollRect>("QuestList").content;
    }

    public override void Init()
    {
        SetupQuestList();
        GetControl<Button>("btnClose").onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            UIManager.Instance.HidePanel<QuestPanel>("QuestPanel", false);
        });
    }

    public void SetupQuestList()
    {
        #region 清理面板内容
        GetControl<Text>("QuestDescription").text = "";
        GetControl<Text>("txtReward").text = "";

        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in rewardTransform)
        {
            if (item.gameObject.name == "txtReward")
                continue;
            Destroy(item.gameObject);
        }
        #endregion

        #region 根据任务管理器，更新任务按钮
        foreach(var task in QuestManager.Instance.tasks)
        {
            QuestButton newTask = Instantiate(btnQuest, content).GetComponent<QuestButton>();
            newTask.SetTaskName(task);
            newTask.questContentText = GetControl<Text>("QuestDescription");
        }
        #endregion
    }

    public void SetupRequirList(QuestData_So data)
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var require in data.questRequires)
        {
            var q = Instantiate(requirement, requireTransform).GetComponent<QuestRequirement>();
            if(data.isComplete)
            {
                q.SetupRequirement(require.name, require.requireAmount, require.requireAmount);
                continue;
            }
            q.SetupRequirement(require.name, require.requireAmount, require.currentAmount);
        }
    }

    public void SetupRewardItem(QuestData_So data)
    {
        GetControl<Text>("txtReward").text = "任务奖励";

        foreach (Transform item in rewardTransform)
        {
            if (item.gameObject.name == "txtReward")
                continue;
            Destroy(item.gameObject);
        }

        foreach (var reward in data.rewards)
        {
            var r = Instantiate(rewardItem, rewardTransform).GetComponent<RewardItem>();
            r.SetupRewardPanel(reward);
        }
    }
}
