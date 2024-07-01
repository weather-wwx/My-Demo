using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    [HideInInspector]
    public Rigidbody2D rigidBody;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private CapsuleCollider2D coll;

    //输入的方向
    public Vector2 inputDirection;

    //物理材质
    private PhysicsMaterial2D normal;
    private PhysicsMaterial2D wall;


    [Header("人物基本参数")]
    //玩家的移动速度
    public float speed;
    //玩家跳跃的力
    public float jumpForce;

    //玩家跳跃力
    public float walljumpForce;
    //玩家被击退的力
    public float hurtForce;

    //滑铲距离
    public float slideDistance;
    //滑铲速度
    public float slideSpeed;

    //碰撞器初始大小 和 偏移
    private Vector2 originalSize;
    private Vector2 originalOffset;


    //状态
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;

    private AudioClip runClip;

    private void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
        coll = this.GetComponent<CapsuleCollider2D>();
        physicsCheck = this.GetComponent<PhysicsCheck>();
        playerAnimation = this.GetComponent<PlayerAnimation>();

        inputControl = new PlayerInputControl();

        //添加跳跃事件
        inputControl.GamePlay.Jump.started += Jump;

        //添加攻击事件
        inputControl.GamePlay.Attack.started += PlayerAttack;

        //添加划铲事件
        inputControl.GamePlay.Slide.started += Slide;
    }

    private void OnEnable()
    {
        inputControl.Enable();  
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Start()
    {
        normal = Resources.Load<PhysicsMaterial2D>("PhysicsMaterial/Normal");
        wall = Resources.Load<PhysicsMaterial2D>("PhysicsMaterial/Wall");
        runClip = Resources.Load<AudioClip>("Music/Sound/Run");
        originalSize = coll.size;
        originalOffset = coll.offset;
    }

    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();

        //改变物理材质
        ChangeState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
        {
            Move();
        }
    }

    //人物移动
    private void Move()
    {
        if(!wallJump)
        {
            rigidBody.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x, rigidBody.velocity.y);
        }

        if(rigidBody.velocity.x !=0 && physicsCheck.isGround)
        {
            MusicController.Instance.PlaySound(runClip);
        }

        //人物翻转
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        this.transform.localScale = new Vector3(faceDir, 1, 1);

        //下蹲
        isCrouch = (inputDirection.y < -0.5f && physicsCheck.isGround) || physicsCheck.TouchTop;
        if(isCrouch)
        {
            coll.size = new Vector2(0.6f, 0.9f);
            coll.offset = new Vector2(0, 0.45f);
        }
        else
        {
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    }

    //跳跃
    private void Jump(InputAction.CallbackContext context)
    {
        if(physicsCheck.isGround)
        {
            //打断滑铲协程
            StopAllCoroutines();
            isSlide = false;

            rigidBody.AddForce(jumpForce * transform.up, ForceMode2D.Impulse);
        }
        //蹬墙跳
        else if(physicsCheck.onWall)
        {
            rigidBody.AddForce(new Vector2(-inputDirection.x, 2.5f) * walljumpForce, ForceMode2D.Impulse);
            this.transform.localScale = new Vector3(-transform.localScale.x, 0, 0);
            wallJump = true;
        }
    }

    //人物攻击
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;

        //进行空中攻击时，给一个向上的力，跟重力抵消
        if (!physicsCheck.isGround)
        {
            rigidBody.AddForce(Vector2.up * 2.5f, ForceMode2D.Impulse);   
        }
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if(!isSlide && physicsCheck.isGround && UIManager.Instance.GetPanel<GamePanel>("GamePanel").slideCounter == 3)
        {
            isSlide = true;
            UIManager.Instance.GetPanel<GamePanel>("GamePanel").slideCounter = 0;

            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y, 0);

            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if(!physicsCheck.isGround)
            {
                break;
            }
            if(physicsCheck.TouchLeftWall && transform.localScale.x < 0 || physicsCheck.TouchRightWall && transform.localScale.x > 0)
            {
                isSlide = false;
                break;
            }

            rigidBody.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        }while(Mathf.Abs(target.x - transform.position.x) > 0.1f);

        gameObject.layer = LayerMask.NameToLayer("Player");
        isSlide = false;
    }


    //人物被破韧后，后退
    public void GetRepel(Transform attacker)
    {
        isHurt = true;
        rigidBody.velocity = Vector2.zero;
        Vector2 dir = new Vector2(this.transform.position.x - attacker.transform.position.x, 0).normalized;

        rigidBody.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }

    public void ChangeState()
    {
        if (isDead || isSlide)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");

        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

        if (physicsCheck.onWall)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 2 / 3);
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
        }

        if(wallJump && rigidBody.velocity.y < 0)
        {
            wallJump = false;
        }
    }
}
