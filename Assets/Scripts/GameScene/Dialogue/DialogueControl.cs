using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueControl : MonoBehaviour
{
    public DialogueData_So currentData;
    private PlayerInputControl inputControl;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && currentData != null)
        {
            inputControl = collision.GetComponent<PlayerController>().inputControl;
            inputControl.GamePlay.LeftPoint.started += OpenDialogue;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inputControl.GamePlay.LeftPoint.started -= OpenDialogue;
        }
    }

    void OpenDialogue(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // 进行射线检测
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("NPC"));
        if(hit.collider != null) 
        {
            //打开UI界面
            //传输对话内容
            DialogueUI.Instance.UpdateDialogueData(currentData);
            DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
        }
    }
}
