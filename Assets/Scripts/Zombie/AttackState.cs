using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackState : State
{
    private PersueTargetState persueTargetState;
    
    [Header("Zombie Attacks")] 
    public ZombieAttackAction[] zombieAttackActions;
    
    [Header("Potential Attacks Performable Right now")]
    public List<ZombieAttackAction> potentialAttacks;

    [Header("Current Attack Being Performed")]
    public ZombieAttackAction currentAttack;

    [Header("State Flags")] 
    public bool hasPerformedAttack;

    private void Awake()
    {
        persueTargetState = GetComponent<PersueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if (!hasPerformedAttack && zombieManager.attackCoolDownTimer <= 0)
        {
            if (currentAttack == null)
            {
                GetNewAttack(zombieManager);
            }
            else
            {
                AttackTarget(zombieManager);
            }
        }

        if (hasPerformedAttack)
        {
            ResetAStateFlags();
            return persueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void GetNewAttack(ZombieManager zombieManager)
    {
        for (int i = 0; i < zombieAttackActions.Length; i++)
        {
            ZombieAttackAction zombieAttack = zombieAttackActions[i];

            if (zombieManager.distanceFromCurrentTarget <= zombieAttack.maximumAttackDistance &&
                zombieManager.distanceFromCurrentTarget >= zombieAttack.minimumAttackDistance)
            {
                if (zombieManager.viewableAngleFromCurretTarget <= zombieAttack.maximumAttackDistance &&
                    zombieManager.viewableAngleFromCurretTarget >= zombieAttack.minimumAttackAngle)
                {
                    potentialAttacks.Add(zombieAttack);
                }
            }
        }

        int randomValue = Random.Range(0, potentialAttacks.Count);

        if (potentialAttacks.Count > 0)
        {
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCoolDownTimer = currentAttack.attackCooldown;
            zombieManager.zombieAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);
        }
        else
        {
            Debug.Log("Warning: Zombie is attempting to perform an attack, but has no attack");
        }
    }

    private void ResetAStateFlags()
    {
        hasPerformedAttack = false;
    }
}
