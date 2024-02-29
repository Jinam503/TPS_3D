using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerManager : CharacterManager
{
    public PlayerLocomotion playerLocomotion;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerNeworkManager playerNeworkManager;
    
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerEquipment playerEquipment;
    [HideInInspector] public PlayerUIManager playerUIManager;
    [HideInInspector] public PlayerAttacker playerAttacker;
    [HideInInspector] public GameMenu gameMenu;
    
    private PlayerStats playerStats;

    public bool isPerformingAction;
    public bool canInteract;
    public bool isDead;

    protected override void Awake()
    {
        base.Awake();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerNeworkManager = GetComponent<PlayerNeworkManager>();
        
        playerEquipment = GetComponent<PlayerEquipment>();
        playerUIManager = GetComponent<PlayerUIManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerStats = GetComponent<PlayerStats>();
        gameMenu = GetComponent<GameMenu>();
    }


    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        playerLocomotion.HandleAllMovement(isPerformingAction);
    }

    protected override void LateUpdate()
    {
        if(!IsOwner)
            return;
        
        base.LateUpdate();
        
        PlayerCamera.instance.HandleAllCameraActions();

        isPerformingAction = animator.GetBool("IsInteracting");
        animator.SetBool("IsDead", isDead);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;

        }
    }
}
