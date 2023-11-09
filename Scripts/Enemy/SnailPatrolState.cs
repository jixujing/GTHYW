using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.findPlayer())
        {
            currentEnemy.SwitchState(NPCState.Skill);
        }

        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.istouchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.istouchRightWall && currentEnemy.faceDir.x > 0)
        {
            currentEnemy.isWait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }

    }

    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        
    }


}


