using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    PlayerAttacker playerAttacker;

    Vector3 moveDir;
    public Transform cameraObject;

    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isDied;

    [Header("Movement Speeds")]
    public float walkingSpeed;
    public float sprintingSpeed;
    public float rotationSpeed;
    public float aimingSpeed;

    [Header("Jump Stats")]
    public float jumpHeight = 1;
    public float gravityIntensity = 10;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }
    
    public void HandleAllMovement(bool isInteracting)
    {
        if (isDied)
        {
            playerRigidbody.velocity = Vector3.zero;
        }
        HandleFallingAndLanding();

        if (isInteracting)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        moveDir = cameraObject.forward * inputManager.verticalInput;
        moveDir += cameraObject.right * inputManager.horizontalInput;
        moveDir.Normalize();
        moveDir.y = 0;

        if (isSprinting)
        {
            moveDir *= sprintingSpeed;
        }
        else
        {
            if(!playerAttacker.isAiming)
            {
                moveDir *= walkingSpeed;
            }
            else
            {
                moveDir *= aimingSpeed;
            }
        }

        if (isGrounded && !playerManager.isInteracting)
        {
            Vector3 movementVelocity = moveDir;
            playerRigidbody.velocity = movementVelocity;
        }
        
    }

    private void HandleRotation()
    {
        if (!isGrounded && !playerManager.isInteracting)
            return;

        Vector3 targetDirection;
        Quaternion targetRotation;
        Quaternion playerRotation;

        if (playerAttacker.isAiming)
        {
            targetDirection = cameraObject.forward;
            targetDirection.Normalize();
            targetRotation = Quaternion.LookRotation(targetDirection);

            playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;

            return;
        }

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        moveDir.y = 0;

        if(targetDirection  == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        targetRotation = Quaternion.LookRotation(targetDirection);
        playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 targetPosition = transform.position;
        raycastOrigin.y += raycastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting && !isDied)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(Vector3.down * fallingVelocity);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out hit, 0.5f, groundLayer))
            //���� ��Ҵ°�
        {
            if (!isGrounded && playerManager.isInteracting && !isDied)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            Vector3 raycastHitPoint = hit.point;
            targetPosition.y = raycastHitPoint.y;
            isGrounded = true;
            playerManager.isInteracting = false;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping)
        { // ���� ��Ҵµ� �������� �ƴϸ�  ��� ��ġ�� �����̵�
            
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (isDied) return;

        if (isGrounded)
        {
            animatorManager.animator.SetBool("IsJumping", true);
            animatorManager.PlayTargetAnimation("Jump", true);

            float jumpingVelocity = Mathf.Sqrt(gravityIntensity * jumpHeight);

            Vector3 playerVelocity = moveDir;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
            
        }
    }

    
}
