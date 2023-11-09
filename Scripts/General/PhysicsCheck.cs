using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    private PlayerController playerController;
    private Rigidbody2D rb;

    [Header("������")]
    public bool manual;//�Ƿ��Զ����
    public bool isPlayer;
    public float CheckRaduis;
    public LayerMask groundLayer;
    public Vector2 bottomOffset;//�ŵ�λ�Ʋ�ֵ
    public Vector2 leftOffset;
    public Vector2 rightOffset;


    [Header("״̬")]  
    public bool isGround;
    public bool istouchLeftWall;
    public bool istouchRightWall;
    public bool onWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x+coll.offset.x)/2 ,coll.bounds.size.y/2 );
            leftOffset = new Vector2(-rightOffset.x, coll.bounds.size.y / 2);
        }

        if (isPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }
    }
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        //������
        if (onWall)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), CheckRaduis, groundLayer);
        }
        else
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), CheckRaduis, groundLayer);
        }
            

        //ǽ����
        istouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, CheckRaduis, groundLayer);
        istouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, CheckRaduis, groundLayer);

        //��ǽ����
        if (isPlayer)
        {
            onWall = (istouchLeftWall&&playerController.inputDirection.x<0f || istouchRightWall&&playerController.inputDirection.x>0f) && rb.velocity.y<0f;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, CheckRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, CheckRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, CheckRaduis);
    }
}
