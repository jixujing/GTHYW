
using UnityEngine;

public class SnailSkillState : BaseState
{


    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("hide", true);
        currentEnemy.anim.SetTrigger("skill");

        currentEnemy.lostTimeCount = currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().invulnerable = true;
        currentEnemy.GetComponent<Character>().invulnerableCount = currentEnemy.lostTimeCount;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCount<= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        currentEnemy.GetComponent<Character>().invulnerableCount = currentEnemy.lostTimeCount;
    }


    public override void PhysicsUpdate()
    {
       
    }   

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        currentEnemy.GetComponent<Character>().invulnerable = false;
    }
}
