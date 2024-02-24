using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    //  The State this character begins on
    public IdleState startingState;
    
    //  The State this character is currently on
    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    
    [Header("Animator")]
    public Animator animator;
    
    [Header("NavMeshAgent")]
    public NavMeshAgent zombieNavMeshAgent;

    [Header("RigidBody")] 
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed;

    private void Awake()
    {
        currentState = startingState;
        animator = GetComponent<Animator>();
        zombieNavMeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void Update()
    {
        zombieNavMeshAgent.transform.localPosition = Vector3.zero;
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
