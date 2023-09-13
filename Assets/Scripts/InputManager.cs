using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool input_LShift;
    public bool input_Ctrl;
    public bool input_Space;

    public bool input_MouseRight;
    public bool input_MouseLeft;

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

            playerControls.PlayerActions.Sprint.performed += i => input_LShift = true;
            playerControls.PlayerActions.Sprint.canceled += i => input_LShift = false;

            playerControls.PlayerActions.Aim.performed += i => input_MouseRight = true;
            playerControls.PlayerActions.Aim.canceled += i => input_MouseRight = false;

            playerControls.PlayerActions.ToggleMovmentMode.performed += i => input_Ctrl = !input_Ctrl;

            playerControls.PlayerActions.Jump.performed += i => input_Space = true;

            playerControls.PlayerActions.Fire.performed += i => input_MouseLeft = true;
            playerControls.PlayerActions.Fire.canceled += i => input_MouseLeft = false;
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
        HandleMovementModeInput();
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
            playerLocomotion.isAiming == false ? 0: horizontalInput,
            playerLocomotion.isAiming == false ? moveAmount : verticalInput,
            playerLocomotion.isSprinting,
            playerLocomotion.movementMode
        );
    }

    private void HandleSprintingInput()
    {
        if (input_LShift)
        {
            playerLocomotion.isSprinting = true;
            if (input_MouseRight)
            {
                playerLocomotion.isSprinting = false;
            }
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleMovementModeInput()
    {
        if (input_Ctrl)
        {
            playerLocomotion.movementMode = true;
        }
        else
        {
            playerLocomotion.movementMode = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (input_Space)
        {
            input_Space = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAimingInput()
    {  
        if (input_MouseRight)
        {
            if (playerLocomotion.isSprinting)
            {
                playerLocomotion.isSprinting = false;
            }
            playerLocomotion.isAiming = true;
            playerLocomotion.movementMode = false;
        }
        else
        {
            playerLocomotion.isAiming = false;
        }
    }

    private void HandleFireInput()
    {
        if (input_MouseLeft)
        {
            playerAttacker.HandleFire(playerInventory.weaponItem);
        }
        else
        {
            playerLocomotion.isFiring = false;
        }
    }
}
