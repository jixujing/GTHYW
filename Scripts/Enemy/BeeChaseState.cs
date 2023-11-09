using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeeChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    private Attack attack;
    private bool isAttack;
    private float attackRateCount=0;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();
        currentEnemy.lostTimeCount = currentEnemy.lostTime;

        currentEnemy.anim.SetBool("chase", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCount <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);

        if(Mathf.Abs(target.x-currentEnemy.transform.position.x)<=attack.attackRange&& Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {    
            isAttack = true;
            if (!currentEnemy.isHurt)
            {
                currentEnemy.rb.velocity = Vector2.zero;
            }
            
            attackRateCount -= Time.deltaTime;
            if (attackRateCount <=0)
            {
                attackRateCount = attack.attackRate;
                currentEnemy.anim.SetTrigger("attack");
            }
        }
        else
        {
            isAttack = false;
        }


        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public override void PhysicsUpdate()
    {
        if (!isAttack && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }  

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false);
    }
}


