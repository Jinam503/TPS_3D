using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    public InputManager inputManager;
    public PlayerLocomotion playerLocomotion;
    public CameraManager cameraManager;
    public AnimatorManager animatorManager;
    public PlayerInventory playerInventory;
    public PlayerEquipment playerEquipment;
    public PlayerUIManager playerUIManager;
    public PlayerAttacker playerAttacker;
    public PlayerStats playerStats;

    public bool isInteracting;
    public bool canInteract;
    public bool isDead;

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
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Debug.Log("Cursor Visible : " + Cursor.visible);
        Debug.Log("Cursor LockState : " + Cursor.lockState);
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
        animator.SetBool("IsDead", isDead);
    }
}
