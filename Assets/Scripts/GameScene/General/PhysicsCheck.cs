using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("״̬")]
    //�Ƿ��ڵ���
    public bool isPlayer;
    public bool isGround;
    public bool TouchLeftWall;
    public bool TouchRightWall;
    public bool TouchTop;
    public bool onWall;

    [Header("������")]
    //��ⷶΧ�뾶
    public float checkRaduis;
    //ƫ��λ��
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public Vector2 topOffset;

    private PlayerController playerController;
    private Rigidbody2D rb;

    private void Awake()
    {
        if(isPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));
        //�������ǽ��
        TouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));
        TouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));

        if(isPlayer)
        {
            //���ͷ���Ƿ�������
            TouchTop = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));
            onWall = (TouchLeftWall && playerController.inputDirection.x < 0 || TouchRightWall && playerController.inputDirection.x > 0) && rb.velocity.y < 0;
        }
    }

    //���Ƽ�ⷶΧ
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, checkRaduis);
    }
}
