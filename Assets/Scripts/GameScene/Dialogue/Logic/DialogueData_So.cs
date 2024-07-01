using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData_So : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
    void OnValidate()
    {
        dialogueIndex.Clear();

        foreach (var piece in dialoguePieces)
        {
            if(!dialogueIndex.ContainsKey(piece.ID))
            {
                dialogueIndex.Add(piece.ID, piece);
            }
        }
    }
#else
    void Awake()//保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }
#endif

    public QuestData_So GetQuest()
    {
        QuestData_So currentQuest = null;
        foreach (var piece in dialoguePieces)
        {
            if(piece.quest != null)
                currentQuest = piece.quest;
        }

        return currentQuest;
    }
}
