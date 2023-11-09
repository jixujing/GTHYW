using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }


    public override void LogicUpdate()
    {
        //·¢ÏÖplayerÇÐ»»µ½chase×´Ì¬
        if (currentEnemy.findPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }


        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.istouchLeftWall &&currentEnemy.faceDir.x < 0 ||currentEnemy.physicsCheck.istouchRightWall && currentEnemy.faceDir.x > 0)
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
        currentEnemy.anim.SetBool("walk", false);
       // Debug.Log("exit");
    }
}
