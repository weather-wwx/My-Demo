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

    //����ķ���
    public Vector2 inputDirection;

    //�������
    private PhysicsMaterial2D normal;
    private PhysicsMaterial2D wall;


    [Header("�����������")]
    //��ҵ��ƶ��ٶ�
    public float speed;
    //�����Ծ����
    public float jumpForce;

    //�����Ծ��
    public float walljumpForce;
    //��ұ����˵���
    public float hurtForce;

    //��������
    public float slideDistance;
    //�����ٶ�
    public float slideSpeed;

    //��ײ����ʼ��С �� ƫ��
    private Vector2 originalSize;
    private Vector2 originalOffset;


    //״̬
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

        //�����Ծ�¼�
        inputControl.GamePlay.Jump.started += Jump;

        //��ӹ����¼�
        inputControl.GamePlay.Attack.started += PlayerAttack;

        //��ӻ����¼�
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

        //�ı��������
        ChangeState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
        {
            Move();
        }
    }

    //�����ƶ�
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

        //���﷭ת
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        this.transform.localScale = new Vector3(faceDir, 1, 1);

        //�¶�
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

    //��Ծ
    private void Jump(InputAction.CallbackContext context)
    {
        if(physicsCheck.isGround)
        {
            //��ϻ���Э��
            StopAllCoroutines();
            isSlide = false;

            rigidBody.AddForce(jumpForce * transform.up, ForceMode2D.Impulse);
        }
        //��ǽ��
        else if(physicsCheck.onWall)
        {
            rigidBody.AddForce(new Vector2(-inputDirection.x, 2.5f) * walljumpForce, ForceMode2D.Impulse);
            this.transform.localScale = new Vector3(-transform.localScale.x, 0, 0);
            wallJump = true;
        }
    }

    //���﹥��
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;

        //���п��й���ʱ����һ�����ϵ���������������
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


    //���ﱻ���ͺ󣬺���
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
