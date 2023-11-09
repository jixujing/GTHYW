using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
  public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("chase", true);

    }
   public override void LogicUpdate()
    {
        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.istouchLeftWall && currentEnemy.faceDir.x < 0 || currentEnemy.physicsCheck.istouchRightWall && currentEnemy.faceDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }

        if (currentEnemy.lostTimeCount <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);

        }

    }


    public override void PhysicsUpdate()
    {


    }

    public override void OnExit()
    {
        currentEnemy.lostTimeCount = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("chase", false);

        
    }
}
