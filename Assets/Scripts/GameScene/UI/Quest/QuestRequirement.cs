using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requirement;
    private Text progressNumber;

    private void Awake()
    {
        requirement = GetComponent<Text>();
        progressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name, int amount, int currentAmount)
    {
        requirement.text = name;
        progressNumber.text = currentAmount.ToString() + "/" + amount.ToString();
    }
}
