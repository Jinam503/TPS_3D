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
    
    [HideInInspector] public PlayerInventory playerInventory;
    [HideInInspector] public PlayerEquipment playerEquipment;
    [HideInInspector] public PlayerUIManager playerUIManager;
    [HideInInspector] public PlayerAttacker playerAttacker;
    [HideInInspector] public GameMenu gameMenu;
    
    private PlayerStats playerStats;

    public bool canInteract;
    public bool isDead;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        
        playerEquipment = GetComponent<PlayerEquipment>();
        playerUIManager = GetComponent<PlayerUIManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerStats = GetComponent<PlayerStats>();
        gameMenu = GetComponent<GameMenu>();

        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;
    }


    protected override void Update()
    {
        base.Update();

        playerLocomotion.HandleAllMovement(isPerformingAction);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        
        PlayerCamera.instance.HandleAllCameraActions();

        isPerformingAction = animator.GetBool("IsInteracting");
        animator.SetBool("IsDead", isDead);
    }
}
