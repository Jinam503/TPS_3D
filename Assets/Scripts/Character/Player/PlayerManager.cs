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
    
    private PlayerStatsManager playerStatsManager;

    public bool canInteract;
    public bool isDead;

    protected override void Awake()
    {
        base.Awake();
        
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        
        playerEquipment = GetComponent<PlayerEquipment>();
        playerUIManager = GetComponent<PlayerUIManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        gameMenu = GetComponent<GameMenu>();

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

        isPerformingAction = animator.GetBool("IsInteracting");
        animator.SetBool("IsDead", isDead);
    }

    public void SaveGameDataToCurrentCharacter(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.xPos = transform.position.x;
        currentCharacterData.yPos = transform.position.y;
        currentCharacterData.zPos = transform.position.z;
    } 
    public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        Vector3 myPos = new Vector3(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
        transform.position = myPos;
    }
}
