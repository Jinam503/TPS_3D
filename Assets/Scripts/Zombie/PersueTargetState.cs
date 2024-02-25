using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PersueTargetState : State
{
    public override State Tick(ZombieManager zombieManager)
    {
        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);
        return this;
    }

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavMeshAgent.enabled = true;
        zombieManager.zombieNavMeshAgent.SetDestination(zombieManager.currentTarget.transform.position);
        Debug.Log("Has Path: " + zombieManager.zombieNavMeshAgent.hasPath);
        zombieManager.transform.rotation = Quaternion.Slerp(
            zombieManager.transform.rotation,
            zombieManager.zombieNavMeshAgent.transform.rotation, 
            zombieManager.rotationSpeed / Time.deltaTime);
        
    }
}
