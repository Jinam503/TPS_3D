using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerAttacker playerAttacker;

    public Transform targetTransform;   // The object the camera will follow
    public Transform cameraPivot;       // The object the camera uses to pivot (look up and down)  
    public Transform cameraTransform;
    public LayerMask collisionLayers;

    private Vector3 cameraVectorPosition;

    private float defaultPosition;

    public float minCollisionOffSet = 0.2f;
    public float cameraCollisionOffset = 0.2f; 
    public float cameraCollisionRadius = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;
    public float pivotAngle;
    public float minPivotAngle = -89f;
    public float maxPivotAngle = 89f;

    private float fireRecoilY;
    private float fireRecoilX;
    private bool firing;
    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        playerAttacker = FindObjectOfType<PlayerManager>().GetComponent<PlayerAttacker>();
        cameraTransform = Camera.main.transform;
        defaultPosition = -1.5f;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        SetDefaultPositionByAiming(); //    Set MaxOffset lower While Aiming ( ���ؽ� ī�޶� ���� )
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = targetTransform.position;

        transform.position = targetPosition; // Move Camera
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle -= (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        if (firing)
        {
            lookAngle += fireRecoilX;
            pivotAngle -= fireRecoilY;
        }

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void SetDefaultPositionByAiming()
    {
        defaultPosition = playerAttacker.isAiming ? -0.6f : -1.5f; // Aimed -> -0.7, Not Aimed -> -2
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        if(Mathf.Abs(targetPosition) < minCollisionOffSet)
        {
            targetPosition = -minCollisionOffSet;
        }
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

    public IEnumerator GunRecoil()
    {
        firing = true;
        fireRecoilX = Random.Range(0.3f, -0.3f);
        float duration = 0.04f; // 전체 회전 소요 시간
        float elapsedTime = 0f;

        float goalTime = Random.Range(2f, 4f);
        
        while (elapsedTime < duration)
        {   
            fireRecoilY = Mathf.Lerp(0, goalTime, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }   

        fireRecoilY = 0f;
        firing = false;
    }
}
