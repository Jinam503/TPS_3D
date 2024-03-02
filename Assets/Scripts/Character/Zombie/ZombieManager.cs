using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public ZombieAnimatorManager zombieAnimatorManager;
    public ZombieStat zombieStat;
    
    //  The State this character begins on
    public IdleState startingState;

    [Header("Flags")] 
    public bool isPerformingAction;
    public bool isDead;
    
    //  The State this character is currently on
    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")] 
    public PlayerManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurretTarget;
    
    [Header("Animator")]
    public Animator animator;
    
    [Header("NavMeshAgent")]
    public NavMeshAgent zombieNavMeshAgent;

    [Header("RigidBody")] 
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed;

    [Header("Attack")] 
    public float attackCoolDownTimer;
    public float minimumAttackDistance;
    public float maximumAttackDistance;

    private void Awake()
    {
        currentState = startingState;
        animator = GetComponent<Animator>();
        zombieNavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
        zombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
        zombieStat = GetComponent<ZombieStat>();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            HandleStateMachine();
        }
    }

    private void Update()
    {
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
