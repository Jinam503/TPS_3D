using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public CharacterManager chracter;

    [Header("Ground Check")] 
    [SerializeField] private float gravityForce = 5;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20;
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallngVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        chracter = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (chracter.isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallngVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if (!fallngVelocityHasBeenSet)
            {
                fallngVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer += Time.deltaTime;

            yVelocity.y += gravityForce * Time.deltaTime;

            chracter.characterController.Move(yVelocity * Time.deltaTime);
        }
    }

    private void HandleGroundCheck()
    {
        chracter.isGrounded = Physics.CheckSphere(chracter.transform.position, groundCheckSphereRadius, groundLayer);
    }
}
