using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_So : ScriptableObject
{
    //任务需求
    [System.Serializable]
    public class QuestRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }

    public string questName;
    [TextArea]
    public string questDescription;

    //任务所处的状态
    public bool isStarted;
    public bool isComplete;
    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();

    public List<ItemInfo> rewards = new List<ItemInfo>();

    public bool CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount == r.currentAmount);
        return isComplete = finishRequires.Count() == questRequires.Count;
    }

    public void GiveRewards()
    {
        foreach(var reward in rewards)
        {
           GameDataMgr.Instance.AddObjItem(GameDataMgr.Instance.GetInfo(reward.id), reward.amount);
        }
    }
}
