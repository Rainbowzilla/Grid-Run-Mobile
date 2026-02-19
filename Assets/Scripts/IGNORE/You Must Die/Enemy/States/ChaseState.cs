using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    public override void Enter()
    {

    }
    public override void Perform()
    {
        if (enemy.Player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);

        // Always move toward player

        // Close enough ? attack
        if (distance <= enemy.attackRange)
        {
            stateMachine.ChangeState(new AttackState());
        }
        
        enemy.Agent.SetDestination(enemy.Player.transform.position);
    }
    public override void Exit()
    {

    }
}
