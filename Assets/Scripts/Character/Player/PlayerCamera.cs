using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;
    public PlayerManager player;
    public Transform cameraPivotTransform;       // The object the camera uses to pivot (look up and down)  

    [Header("Camera Settings")] 
    [SerializeField] private float leftAndRightRotationSpeed;
    [SerializeField] private float upAndDownRotationSpeed;
    [SerializeField] private float minimumPivot = -30;
    [SerializeField] private float maximumPivot = 60;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collisionLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameObjectPosition;
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    private float defaultCameraPosition;
    private float targetCameraPosition;

    private float fireRecoilY;
    private float fireRecoilX;
    private bool firing;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        defaultCameraPosition = cameraObject.transform.localPosition.z;
    }
    
    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            FollowTarget();
            RotateCamera();
            SetDefaultPositionByAiming(); //    Set MaxOffset lower While Aiming ( ���ؽ� ī�޶� ���� )
            HandleCameraCollisions();
        }
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = player.transform.position;

        transform.position = targetPosition; // Move Camera
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        rotation = Vector3.zero;
        rotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void SetDefaultPositionByAiming()
    {
        defaultCameraPosition = player.playerAttacker.isAiming ? -0.6f : -1.5f; // Aimed -> -0.7, Not Aimed -> -2
    }

    private void HandleCameraCollisions()
    {
        targetCameraPosition = defaultCameraPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(
                cameraPivotTransform.transform.position,
                cameraCollisionRadius,
                direction,
                out hit,
                Mathf.Abs(targetCameraPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraPosition = -(distance - cameraCollisionRadius);
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        if(Mathf.Abs(targetCameraPosition) < cameraCollisionRadius)
        {
            targetCameraPosition = -cameraCollisionRadius;
        }
        cameObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraPosition, 0.2f);
        cameraObject.transform.localPosition = cameObjectPosition;
    }
}
