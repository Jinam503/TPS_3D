using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    private PlayerManager playerManager;
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

    public bool inputReload;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerManager = GetComponent<PlayerManager>();
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

            playerControls.PlayerActions.Reload.performed += i => inputReload = true;
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
        HandleReloadInput();
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
        if (inputLShift && moveAmount >0f)
        {
            if (!playerAttacker.isReloading)
            {
                playerAttacker.rightHandRigWeight = 1f;
            }
            playerLocomotion.isSprinting = true;
            if (inputMouseRight)
            {
                playerLocomotion.isSprinting = false;
            }
        }
        else
        {
            playerAttacker.rightHandRigWeight = 0f;
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

            if (!playerAttacker.isReloading)
            {
                playerAttacker.aimRigWeight = 1f;
                playerAttacker.rightHandRigWeight = 1f;
            }
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
        if (inputMouseLeft && !playerAttacker.isReloading)
        {
            playerAttacker.isFiring = true;
        }
        else
        {
            playerAttacker.isFiring = false;
        }
        playerAttacker.HandleFire(playerInventory.weaponItem);
    }

    private void HandleReloadInput()
    {
        if (inputReload &&
            !playerAttacker.isReloading &&
            !playerAttacker.isFiring &&
            playerInventory.weaponSlotManager.ReturnCurrentWeaponItemInHandSlot().remainingAmmo != 20 &&
            !playerLocomotion.isDied)
        {
            inputReload = false;
            playerAttacker.HandleReload();
        }
    }
}
