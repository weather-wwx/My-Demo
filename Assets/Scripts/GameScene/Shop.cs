using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    private PlayerInputControl inputControl;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inputControl = collision.GetComponent<PlayerController>().inputControl;
            inputControl.GamePlay.LeftPoint.started += OpenShop;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inputControl.GamePlay.LeftPoint.started -= OpenShop;
        }
    }

    void OpenShop(InputAction.CallbackContext context)
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            // 进行射线检测
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Shop"));
            if (hit.collider != null)
            {
                //打开Shop界面
                UIManager.Instance.ShowPanel<ShopPanel>("ShopPanel", false);
            }
        }
    }
}
