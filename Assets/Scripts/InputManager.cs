using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

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

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
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

            playerControls.PlayerActions.ToggleMovmentMode.performed += i => input_Ctrl = !input_Ctrl;

            playerControls.PlayerActions.Jump.performed += i => input_Space = true;
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
        //HandleActionInput
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting, playerLocomotion.movementMode);
    }

    private void HandleSprintingInput()
    {
        if (input_LShift)
        {
            playerLocomotion.isSprinting = true;
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
}
