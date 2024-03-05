using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : CharacterManager
{
    public ZombieAnimatorManager zombieAnimatorManager;
    public ZombieStat zombieStat;
    
    //  The State this character begins on
    public IdleState startingState;
    
    //  The State this character is currently on
    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")] 
    public CharacterManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurretTarget;
    
    [Header("NavMeshAgent")]
    public NavMeshAgent zombieNavMeshAgent;

    [Header("Locomotion")]
    public float rotationSpeed;

    [Header("Attack")] 
    public float attackCoolDownTimer;
    public float minimumAttackDistance;
    public float maximumAttackDistance;

    protected override void Awake()
    {
        base.Awake();
        currentState = startingState;
        zombieNavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
        zombieStat = GetComponent<ZombieStat>();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (!isDead)
        {
            HandleStateMachine();
        }
    }
    protected override void Update()
    {
        base.Update();
        
        zombieNavMeshAgent.transform.localPosition = Vector3.zero;

        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer = attackCoolDownTimer - Time.deltaTime;
        }

        if (currentTarget != null)
        {
            targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurretTarget = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void HandleStateMachine()
    {
        State nextState;
        
        if (currentState != null)
        {
            nextState = currentState.Tick(this);
            
            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }

    
}
