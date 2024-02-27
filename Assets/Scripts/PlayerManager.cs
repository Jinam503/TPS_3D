using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    
    public PlayerLocomotion playerLocomotion;
    public CameraManager cameraManager;
    public AnimatorManager animatorManager;
    public PlayerInventory playerInventory;
    public PlayerEquipment playerEquipment;
    public PlayerUIManager playerUIManager;
    public PlayerAttacker playerAttacker;

    public bool isInteracting;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponentInChildren<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();;
        playerEquipment = GetComponent<PlayerEquipment>();
        playerUIManager = GetComponent<PlayerUIManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        // Player Movement with Rigidbody should be in FixedUpdate. Its kinda rule of Unity;
        playerLocomotion.HandleAllMovement(isInteracting);
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("IsInteracting");
        //playerLocomotion.isJumping = animator.GetBool("IsJumping");
        animator.SetBool("IsGrounded", playerLocomotion.isGrounded);
    }
}
