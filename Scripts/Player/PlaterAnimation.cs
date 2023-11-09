using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaterAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck PhysicsCheck;
    private PlayerController player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PhysicsCheck = GetComponent<PhysicsCheck>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        anim.SetFloat("velocityX" , Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", PhysicsCheck.isGround);
        anim.SetBool("isDead", player.isDead);
        anim.SetBool("isAttack",player.isAttack);
        anim.SetBool("isWall", PhysicsCheck.onWall);
        anim.SetBool("isSlide", player.isSlide);
    }

    public void PlayerHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
       
    }
}
