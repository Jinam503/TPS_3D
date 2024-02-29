using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;
    
    PlayerControls playerControls;
    
    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("PLAYER CAMERA INPUT")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;
    
    [Header("PLAYER ACTION INPUT")]
    public bool runInput;
    public bool inputSpace;
    public bool aimInput;
    public bool fireInput;
    public bool reloadInput;
    public bool interactInput;
    public bool openMenuInput;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = GetComponent<PlayerManager>();

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        //  WHEN SCENE CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += OnSceneChange;
        
        instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
            
            
            player.transform.position = FindObjectOfType<SpawnPoint>().transform.position;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => runInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => runInput = false;

            playerControls.PlayerActions.Aim.performed += i => aimInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimInput = false;

            playerControls.PlayerActions.Jump.performed += i => inputSpace = true;

            playerControls.PlayerActions.Fire.performed += i => fireInput = true;
            playerControls.PlayerActions.Fire.canceled += i => fireInput = false;

            playerControls.PlayerActions.Reload.performed += i => reloadInput = true;

            playerControls.PlayerActions.Interact.performed += i => interactInput = true;

            playerControls.PlayerActions.OpenInventory.performed += i => openMenuInput = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged-= OnSceneChange;
    }
    private void OnDisable()
    {
        //  IF WE DESTROY THIS OBJECT, UNSUBSCRIE FROM THIS EVENT
        playerControls.Disable();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (enabled)
        {
            if (hasFocus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    public void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleRunning();
        HandleAiming();
        
        HandleFireInput();
        HandleReloadInput();
        HandleInteractionInput();
        HandleOpenMenuInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (player == null)
            return;
        
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0,moveAmount, player.playerNeworkManager.isRunning.Value);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void HandleRunning()
    {
        
        if (runInput && !aimInput)
        {
            player.playerLocomotion.HandleRunning();
        }
        else
        {
            player.playerNeworkManager.isRunning.Value = false;
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

    private void HandleAiming()
    {
        if (aimInput)
        {
            if (!player.playerAttacker.isReloading)
            {
                player.playerAttacker.aimRigWeight = 1f;
                player.playerAttacker.rightHandRigWeight = 1f;
            }
            player.playerAttacker.isAiming = true;
        }
        else
        {
            player.playerAttacker.aimRigWeight = 0f;
            player.playerAttacker.isAiming = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleFireInput()
    {
        if (fireInput && !player.playerAttacker.isFiring)
        {
            fireInput = false;
            player.playerAttacker.isFiring = true;
        }
        player.playerAttacker.HandleFire(player.playerEquipment.weaponItem);
    }

    private void HandleReloadInput()
    {
        if (reloadInput &&
            !player.playerAttacker.isReloading &&
            !player.playerAttacker.isFiring &&
            !player.isDead)
        {
            reloadInput = false;
            player.playerAttacker.HandleReload();
        }
    }

    private void HandleInteractionInput()
    {
        if (interactInput)
        {
            if (!player.canInteract)
            {
                interactInput = false;
                player.canInteract = false;
            }
        }
    }
    private void HandleOpenMenuInput()
    {
        if (openMenuInput)
        {
            openMenuInput = false;
            player.gameMenu.HandleMenu();
        }
    }
}