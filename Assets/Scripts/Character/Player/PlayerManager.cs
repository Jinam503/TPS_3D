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
    [HideInInspector] public PlayerAttacker playerAttacker;
    
    private PlayerStatsManager playerStatsManager;

    public bool canInteract;

    protected override void Awake()
    {
        base.Awake();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        
        playerEquipment = GetComponent<PlayerEquipment>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    private void Start()
    {
        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;
        WorldSaveGameManager.instance.player = this;
    }

    protected override void Update()
    {
        base.Update();

        playerLocomotion.HandleAllMovement();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        
        PlayerCamera.instance.HandleAllCameraActions();
        
        //  ANIMATION PARAMETERS
        isPerformingAction = animator.GetBool("IsInteracting");
        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsAiming", playerAttacker.isAiming);
        animator.SetBool("IsFiring", playerAttacker.isFiring);
        animator.SetBool("IsReloading", playerAttacker.isReloading);
    }

    public void SaveGameDataToCurrentCharacter(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.xPos = transform.position.x;
        currentCharacterData.yPos = transform.position.y;
        currentCharacterData.zPos = transform.position.z;

        currentCharacterData.health = playerStatsManager.currentHealth;
    } 
    public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        Vector3 myPos = new Vector3(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
        transform.position = myPos;

        playerStatsManager.currentHealth = currentCharacterData.health;
    }
}
