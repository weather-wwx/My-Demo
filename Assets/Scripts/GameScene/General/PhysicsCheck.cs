using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("×´Ì¬")]
    //ÊÇ·ñ´¦ÔÚµØÃæ
    public bool isPlayer;
    public bool isGround;
    public bool TouchLeftWall;
    public bool TouchRightWall;
    public bool TouchTop;
    public bool onWall;

    [Header("¼ì²â²ÎÊý")]
    //¼ì²â·¶Î§°ë¾¶
    public float checkRaduis;
    //Æ«ÒÆÎ»ÖÃ
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
        //¼ì²âµØÃæ
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));
        //¼ì²â×óÓÒÇ½Ìå
        TouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));
        TouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));

        if(isPlayer)
        {
            //¼ì²âÍ·¶¥ÊÇ·ñÓÐÎïÌå
            TouchTop = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, checkRaduis, 1 << LayerMask.NameToLayer("Ground"));
            onWall = (TouchLeftWall && playerController.inputDirection.x < 0 || TouchRightWall && playerController.inputDirection.x > 0) && rb.velocity.y < 0;
        }
    }

    //»æÖÆ¼ì²â·¶Î§
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, checkRaduis);
    }
}
