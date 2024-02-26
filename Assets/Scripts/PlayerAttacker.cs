using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    private AnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerInventory playerInventory;

    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig leftHandRig;

    [Header("WeaponBulletRange")] 
    public float bulletRange = 100f;

    [Header("Shootable Layer")]
    public LayerMask shootableLayers;

    public bool isAiming;
    public bool isFiring;

    public float aimRigWeight;
    public float rightHandRigWeight;

    private float shootTimer;
    private float shootTimerMax = 0.1f;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
    }
    private void Update()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        leftHandRig.weight = Mathf.Lerp(leftHandRig.weight, rightHandRigWeight, Time.deltaTime * 20f);
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
                    if (Physics.Raycast(muzzleSpawnPosition, direction, out hit, bulletRange, shootableLayers))
                    {
                        Debug.DrawLine(muzzleSpawnPosition, hit.point, Color.red, 1);
                        Debug.Log(hit.collider.gameObject.layer);
                        ZombieEffectManager zombie =
                            hit.collider.gameObject.GetComponentInParent<ZombieEffectManager>();
                        if (zombie != null)
                        {
                            int damage = playerInventory.weaponSlotManager.ReturnCurrentWeaponItemInHandSlot().damage;
                            if (hit.collider.gameObject.layer == 8)
                            {
                                zombie.DamageZombieHead(damage);
                            }
                            else if (hit.collider.gameObject.layer == 9)
                            {
                                zombie.DamageZombieTorso(damage);
                            }
                            else if (hit.collider.gameObject.layer == 10)
                            {
                                zombie.DamageZombieLeftArm(damage);
                            }
                            else if (hit.collider.gameObject.layer == 11)
                            {
                                zombie.DamageZombieRightArm(damage);
                            }
                            else if (hit.collider.gameObject.layer == 12)
                            {
                                zombie.DamageZombieLeftLeg(damage);
                            }
                            else if (hit.collider.gameObject.layer == 13)
                            {
                                zombie.DamageZombieRighLeg(damage);   
                            }
                        }
                    }
                }
            }
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
