using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    [SerializeField] private Rig aimRig;

    [SerializeField] private Transform ifNoRayCastHit;

    public bool isAiming;
    public bool isFiring;

    public float aimRigWeight;

    private float shootTimer;
    private float shootTimerMax = 0.1f;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Update()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }

    private void FixedUpdate()
    {
        HandleAiming();
        HandleFiring();
    }


    public void HandleFire(WeaponItem weaponItem)
    {
        if (isFiring)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                shootTimer += shootTimerMax + Random.Range(0f, shootTimerMax * 0.25f);
                
                //  When Aiming and player Alive
                if (isAiming && !playerLocomotion.isDied) 
                {   //  Play Fire Animation
                    animatorManager.PlayTargetAnimation(weaponItem.Rifle_Fire, false);
                    
                    //  Spawn Muzzle Effect
                    Vector3 muzzleSpawnPosition = weaponItem.muzzleSpawnPosition.position;
                    
                    Transform muzzleFlash = Instantiate(weaponItem.muzzleFlashPrefab, muzzleSpawnPosition, Quaternion.identity). transform;
                    muzzleFlash.parent = weaponItem.muzzleSpawnPosition;
                    
                    //Check Hit
                    
                    RaycastHit hit;
                    Vector3 direction = (Mouse3D.GetMouseWorldPosition() - muzzleSpawnPosition).normalized;
                    if (Physics.Raycast(muzzleSpawnPosition, direction, out hit))
                    {
                        Debug.Log(hit.transform.name);
                    }
                }
            }
        }
        else
        {
            //weaponItem.muzzleFlash.Emit(0);
        }
    }

    private void HandleAiming()
    {
        animatorManager.animator.SetBool("IsAiming", isAiming);
    }

    private void HandleFiring()
    {
        animatorManager.animator.SetBool("IsFiring", isFiring);
    }
}
