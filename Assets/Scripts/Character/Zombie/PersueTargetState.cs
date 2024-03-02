using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PersueTargetState : State
{
    private AttackState attackState;

    private void Awake()
    {
        attackState = GetComponent<AttackState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }
        
        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);
        if (zombieManager.distanceFromCurrentTarget <= zombieManager.maximumAttackDistance)
        {
            zombieManager.zombieNavMeshAgent.enabled = false;
            return attackState;
        }
        else
        {
            return this;
        }
    }

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavMeshAgent.enabled = true;
        zombieManager.zombieNavMeshAgent.SetDestination(zombieManager.currentTarget.transform.position);
        zombieManager.transform.rotation = Quaternion.Slerp(
            zombieManager.transform.rotation,
            zombieManager.zombieNavMeshAgent.transform.rotation, 
            zombieManager.rotationSpeed / Time.deltaTime);
        
    }
}
