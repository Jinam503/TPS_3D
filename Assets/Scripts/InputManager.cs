using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;

    [SerializeField]
    private  Vector2 movementInput;

    [SerializeField]
    private Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;

    public float verticalInput;
    public float horizontalInput;

    public bool inputLShift;
    public bool inputSpace;

    public bool inputMouseRight;
    public bool inputMouseLeft;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => inputLShift = true;
            playerControls.PlayerActions.Sprint.canceled += i => inputLShift = false;

            playerControls.PlayerActions.Aim.performed += i => inputMouseRight = true;
            playerControls.PlayerActions.Aim.canceled += i => inputMouseRight = false;

            playerControls.PlayerActions.Jump.performed += i => inputSpace = true;

            playerControls.PlayerActions.Fire.performed += i => inputMouseLeft = true;
            playerControls.PlayerActions.Fire.canceled += i => inputMouseLeft = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleAimingInput();
        HandleFireInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(
            playerAttacker.isAiming == false ? 0: horizontalInput,
            playerAttacker.isAiming == false ? moveAmount : verticalInput,
            playerLocomotion.isSprinting
        );
    }

    private void HandleSprintingInput()
    {
        if (inputLShift)
        {
            playerLocomotion.isSprinting = true;
            if (inputMouseRight)
            {
                playerLocomotion.isSprinting = false;
            }
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (inputSpace)
        {
            inputSpace = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAimingInput()
    {  
        if (inputMouseRight && !playerLocomotion.isJumping)
        {
            if (playerLocomotion.isSprinting)
            {
                playerLocomotion.isSprinting = false;
            }
            playerAttacker.aimRigWeight = 1f;
            playerAttacker.isAiming = true;
        }
        else
        {
            playerAttacker.aimRigWeight = 0f;
            playerAttacker.isAiming = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleFireInput()
    {
        if (inputMouseLeft)
        {
            playerAttacker.isFiring = true;
        }
        else
        {
            playerAttacker.isFiring = false;
        }
        playerAttacker.HandleFire(playerInventory.weaponItem);
    }
}
