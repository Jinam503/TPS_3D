using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    Vector3 moveDir;
    public Transform cameraObject;
    Rigidbody playerRigidbody;

    public bool isSprinting;
    public bool movementMode; // true => run, false => walk

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();

    }
    
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDir = cameraObject.forward * inputManager.verticalInput;
        moveDir += cameraObject.right * inputManager.horizontalInput;
        moveDir.Normalize();
        moveDir.y = 0;

        if (isSprinting)
        {
            moveDir *= sprintingSpeed;
        }
        else
        {
            if (movementMode)
            {
                moveDir *= runningSpeed;
            }
            else
            {
                moveDir *= walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDir;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        moveDir.y = 0;

        if(targetDirection  == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    
}
