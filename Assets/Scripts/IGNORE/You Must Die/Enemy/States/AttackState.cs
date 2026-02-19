using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState
{
    //private float attackCooldown = 1.2f;
    private float timer;

    //public float attackRange = 2f;

    public override void Enter()
    {
        timer = enemy.attackCooldown;
        //enemy.Agent.ResetPath(); // stop moving

        // Fully stop movement
        enemy.Agent.isStopped = true;
        enemy.Agent.velocity = Vector3.zero;
        Debug.Log("Zombie stopped moving");
    }

    public override void Perform()
    {
        if (enemy.Player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);

        // If player moved away ? chase again
        if (distance > enemy.attackRange)
        {
            stateMachine.ChangeState(new ChaseState());
            return;
        }

        // Face player
        Vector3 lookPos = enemy.Player.transform.position;
        lookPos.y = enemy.transform.position.y;
        enemy.transform.LookAt(lookPos);

        // Countdown to hit
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            DealDamage();
            timer = enemy.attackCooldown;
        }
    }

    public override void Exit() 
    {
        // Allow movement again when leaving attack
        enemy.Agent.isStopped = false;
    }

    void DealDamage()
    {
        //Debug.Log("Zombie hit player");
        enemy.Player.GetComponent<PlayerHealth>().TakeDamage(enemy.attackDamage);
    }
}


