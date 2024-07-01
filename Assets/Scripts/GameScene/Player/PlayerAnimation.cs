using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private Player player;
    private PlayerController playerController;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        physicsCheck = this.GetComponent<PhysicsCheck>();
        player = this.GetComponent<Player>();
        playerController = this.GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        animator.SetBool("isAttack", playerController.isAttack);
        animator.SetBool("onWall", physicsCheck.onWall);
        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isGround", physicsCheck.isGround);
        animator.SetBool("isCrouch", playerController.isCrouch);
        animator.SetBool("isRepel", player.isRepel);
        animator.SetBool("isSlide", playerController.isSlide);
        animator.SetBool("isDead", playerController.isDead);
        
    }

    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void PlayAttack()
    {
        animator.SetTrigger("attack");
    }
}
