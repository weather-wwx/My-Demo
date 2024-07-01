using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    private static DialogueUI instance;
    public  static DialogueUI Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Basic Elements")]
    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;

    [Header("Options")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")]
    public DialogueData_So currentData;

    public int currentIndex = 0;

    void Awake()
    {
        instance = this;
        dialoguePanel.SetActive(false);
    }

    void Start()
    {
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    void ContinueDialogue()
    {
        if(currentIndex < currentData.dialoguePieces.Count)
        {
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        }
        else 
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void UpdateDialogueData(DialogueData_So data)
    {
        currentData = data;
        currentIndex = 0;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;

        if (piece.image!=null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        mainText.text = "";
        mainText.DOText(piece.text, 0.8f);

        if(piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        CreatOptions(piece);
    }

    void CreatOptions(DialoguePiece piece)
    {

        //销毁之前的选项按钮
        if(optionPanel.childCount > 0)
        {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }

        //创建新的选项按钮
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece, piece.options[i]);
        }
    }
}
