using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;

    private Button thisButton;
    private DialoguePiece currentPiece;

    private string nextPieceId;
    private bool takeQuest;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        currentPiece = piece;
        optionText.text = option.text;
        nextPieceId = option.targetID;
        takeQuest = true;
    }

    public void OnOptionClicked()
    {
        if(currentPiece.quest != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)
            };

            if(takeQuest)
            {
                //��ӵ������б���
                //�ж��Ƿ��Ѿ�������
                if(QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //�ж��Ƿ����
                    if(QuestManager.Instance.GetTask(newTask.questData).isComplete)
                    {
                        newTask.questData.GiveRewards();
                        QuestManager.Instance.GetTask(newTask.questData).isFinished = true;
                    }
                }
                else
                {
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).isStarted = true;
                    QuestManager.Instance.SaveTaskData();
                }
            }
        }

        if(nextPieceId == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceId]);
        }
    }
}
