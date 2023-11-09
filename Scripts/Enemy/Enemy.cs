using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{

    [HideInInspector] public Animator anim;
    [HideInInspector]public Rigidbody2D rb;
    public Vector3 faceDir;
    [HideInInspector] public PhysicsCheck physicsCheck;


    [Header("基本属性")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Transform attacker;
    public float hurtForce;
    public float hurtTime;
    public Vector3 spwanPoint;

    [Header("计时器")]
    public float waitTime;
    public float waitTimeCount;
    public bool isWait;
    public float lostTime;
    public float lostTimeCount;



    [Header("状态")]
    public bool isHurt;
    public bool isDead;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackerLayer;


    protected BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState skillState;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed;
        waitTimeCount = waitTime;
        spwanPoint = transform.position;

    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x,0,0);
           
        currentState.LogicUpdate(); 
        TimeCount();
    }

    private void FixedUpdate()
    {
        if(!isHurt&&!isDead&&!isWait)
        Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnEnter(this);    
    }


    public virtual void Move()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("PreMove")&& !anim.GetCurrentAnimatorStateInfo(0).IsName("SnailRecover"))
        rb.velocity = new Vector2(currentSpeed * Time.deltaTime * faceDir.x, rb.velocity.y);
    }


    //计时器
    public void TimeCount()
    {
        
        if (isWait)
        {            
            waitTimeCount = waitTimeCount - Time.deltaTime;
            if (waitTimeCount<= 0)
            {
                isWait = false;
                waitTimeCount = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        if (!findPlayer() && lostTimeCount>0)
        {
            lostTimeCount -= Time.deltaTime;
        }

    }

    public virtual bool findPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize,0,faceDir, checkDistance,attackerLayer);
       
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill =>skillState,
            _ => null

        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }

    #region 事件执行方法

    public void OnTakeDamage(Transform attackTran)
    {
        attacker = attackTran;

        //转身
        if(attacker.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if(attacker.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.velocity = new Vector2(0,rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    IEnumerator OnHurt(Vector2 dir)
    {

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(hurtTime);
        isHurt = false;
    }

    public void OnDie()
    {
       
        anim.SetBool("dead", isDead);
        isDead = true;

    }

    public void DestroyAfterAnimation()
    {
        gameObject.layer = 2;
        Destroy(this.gameObject);
    }


    #endregion

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position +(Vector3)centerOffset +new Vector3(checkDistance*-transform.lossyScale.x,0,1), 0.2f);
    }
}
