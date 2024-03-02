using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
    private PlayerManager player;


    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;


    [Header("MOVEMENT SETTINGS")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float aimingSpeed;

    [Header("FLAGS")] 
    public bool isRunning;
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();
        ;
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, isRunning);
    }

    public void HandleAllMovement()
    {
        if(player.isPerformingAction)
           return;
        
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
            return;
        
        GetMovementValues();
        
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isRunning)
        {
            player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else
        {
            if (!player.playerAttacker.isAiming)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
            else
            {
                player.characterController.Move(moveDirection * aimingSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
            return;
        
        Quaternion newRotation;
        Quaternion targetRotation;

        if (player.playerAttacker.isAiming)
        {
            targetRotationDirection = PlayerCamera.instance.transform.forward;
        }
        else
        {
            targetRotationDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        }
        
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        newRotation = Quaternion.LookRotation(targetRotationDirection);
        targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
    

    public void HandleRunning()
    {
        if (player.isPerformingAction)
        {
            isRunning = false;
        }

        if (moveAmount >= 0.9f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
}
