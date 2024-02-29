using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : CharacterLocomotion
{
    private PlayerManager player;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;


    [Header("Movement Speeds")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float aimingSpeed;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();
        ;
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNeworkManager.isRunning.Value);
        }
    }

    public void HandleAllMovement(bool isInteracting)
    {
        if (isInteracting)
        {
            return;
        }

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
        GetMovementValues();
        
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.instance.runInput)
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
            player.playerNeworkManager.isRunning.Value = false;
        }

        if (moveAmount >= 0.9f)
        {
            player.playerNeworkManager.isRunning.Value = true;
        }
        else
        {
            player.playerNeworkManager.isRunning.Value = false;
        }
    }
}
