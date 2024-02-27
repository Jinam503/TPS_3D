using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    private PlayerManager playerManager;
    PlayerControls playerControls;

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
    public bool inputInteract;

    private void Awake()
    {
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

            playerControls.PlayerActions.Interact.performed += i => inputInteract = true;
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
        HandleAimingInput();
        HandleFireInput();
        HandleReloadInput();
        HandleInteractionInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        playerManager.animatorManager.UpdateAnimatorValues(
            playerManager.playerAttacker.isAiming == false ? 0: horizontalInput,
            playerManager.playerAttacker.isAiming == false ? moveAmount : verticalInput,
            playerManager.playerLocomotion.isSprinting
        );
    }

    private void HandleSprintingInput()
    {
        if (inputLShift && moveAmount >0f)
        {
            if (!playerManager.playerAttacker.isReloading)
            {
                playerManager.playerAttacker.rightHandRigWeight = 1f;
            }
            playerManager.playerLocomotion.isSprinting = true;
            if (inputMouseRight)
            {
                playerManager.playerLocomotion.isSprinting = false;
            }
        }
        else
        {
            playerManager.playerAttacker.rightHandRigWeight = 0f;
            playerManager.playerLocomotion.isSprinting = false;
        }
    }

    /*private void HandleJumpingInput()
    {
        if (inputSpace)
        {
            inputSpace = false;
            playerManager.playerLocomotion.HandleJumping();
        }
    }*/

    private void HandleAimingInput()
    {  
        if (inputMouseRight)
        {
            if (playerManager.playerLocomotion.isSprinting)
            {
                playerManager.playerLocomotion.isSprinting = false;
            }

            if (!playerManager.playerAttacker.isReloading)
            {
                playerManager.playerAttacker.aimRigWeight = 1f;
                playerManager.playerAttacker.rightHandRigWeight = 1f;
            }
            playerManager.playerAttacker.isAiming = true;
        }
        else
        {
            playerManager.playerAttacker.aimRigWeight = 0f;
            playerManager.playerAttacker.isAiming = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleFireInput()
    {
        if (inputMouseLeft && !playerManager.playerAttacker.isFiring)
        {
            inputMouseLeft = false;
            playerManager.playerAttacker.isFiring = true;
        }
        playerManager.playerAttacker.HandleFire(playerManager.playerEquipment.weaponItem);
    }

    private void HandleReloadInput()
    {
        if (inputReload &&
            !playerManager.playerAttacker.isReloading &&
            !playerManager.playerAttacker.isFiring &&
            !playerManager.isDead)
        {
            inputReload = false;
            playerManager.playerAttacker.HandleReload();
        }
    }

    private void HandleInteractionInput()
    {
        if (inputInteract)
        {
            if (!playerManager.canInteract)
            {
                inputInteract = false;
            }
        }
    }
}
