using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSo afterSceneLoadEvent;
    public VoidEventSo loadDataEvent;
    public VoidEventSo backToMenuEvent;


    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlaterAnimation playerAnimation;
    private Collider2D coll;
    private Character character;

    [Header("�����������")]
    public float speed;
    public float jumpFroce;
    public float hurtFroce;
    public float wallJumpForce;
    public float slideDistance;
    public float slideSpeed;
    public int SlidePowerCost;

    [Header("״̬")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;


    [Header("����")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D smooth;


    [Header("���Ի�ȡ")]
    public Transform Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();

        inputControl.Gameplay.Jump.started += Jump;    //������Ծ�йغ���
        inputControl.Gameplay.Attack.started += Attack;//���������йغ���
        inputControl.Gameplay.Slide.started += Slide;

        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlaterAnimation>();
        coll = GetComponent<Collider2D>();
        character = GetComponent<Character>();

        inputControl.Enable();

        Player = this.gameObject.transform;
    }



    private void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }



    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }



    //���ع��̺���������
    private void OnAfterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable();
    }

    //���ع�����ֹͣ����
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }

    //��ȡ��Ϸ����
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        checkState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
        Move();
    }

/*    //����
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }*/
    private void Move()
    {
       
        if (!wallJump&&!isSlide&&!isAttack)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }
       
        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0 && !isSlide)
        {
            faceDir = 1;
        }
           
        if (inputDirection.x < 0 && !isSlide)
        {
            faceDir = -1;
        }
              //���﷭ת
            transform.localScale = new Vector3(faceDir, 1, 1);

         

    }

    //Jump����
    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpFroce, ForceMode2D.Impulse);
            isSlide = false;
            StopAllCoroutines();
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
            
       else if (physicsCheck.onWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x , 2.8f)*wallJumpForce,ForceMode2D.Impulse);
            wallJump = true;
        }
    }

   // attack����
    private void Attack(InputAction.CallbackContext obj)
    {

        playerAnimation.PlayerAttack();
        isAttack = true;

    }
    private void Slide(InputAction.CallbackContext obj)
    {
        if (!isSlide && physicsCheck.isGround&&character.currentPower>=SlidePowerCost)
        {
            isSlide = true;
            var targetPos = new Vector2(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);

            gameObject.layer = LayerMask.NameToLayer("Enemy");

            StartCoroutine(TriggerSlide(targetPos));

            character.onSlide(SlidePowerCost);
        }
        
    }

    private IEnumerator TriggerSlide(Vector2 target)
    {
        do
        {
            yield return null;
            if (!physicsCheck.isGround)
            {
                break;
            }
            if (physicsCheck.istouchLeftWall || physicsCheck.istouchRightWall)
            {
                isSlide = false;
                break;
            }

                rb.MovePosition(new Vector2(transform.position.x + slideSpeed * transform.localScale.x, transform.position.y));



        } while (Mathf.Abs(target.x - transform.position.x) > 0.1f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        
    }


    public void getHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;

        rb.AddForce(dir * hurtFroce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    private void checkState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : smooth;

        if (physicsCheck.onWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2);
            
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        if (wallJump && rb.velocity.y<0f)
        {
            wallJump = false;
        }
    }

    
}
