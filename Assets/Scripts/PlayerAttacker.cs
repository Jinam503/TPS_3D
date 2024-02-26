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
    private PlayerUIManager playerUIManager;

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
    private float shootTimerMax = 0.7f;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
        playerUIManager = GetComponent<PlayerUIManager>();
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
                if (playerInventory.weaponSlotManager.ReturnCurrentWeaponItemInHandSlot().remainingAmmo > 0)
                {
                    shootTimer += shootTimerMax;
                    
                    //  When Aiming and player Alive
                    if (isAiming && !playerLocomotion.isDied) 
                    {   
                        //  Minus Bullet from magazine
                        playerInventory.weaponSlotManager.ReturnCurrentWeaponItemInHandSlot().remainingAmmo--;
                        playerUIManager.currentAmmoCountText.text = playerInventory.weaponSlotManager
                            .ReturnCurrentWeaponItemInHandSlot().remainingAmmo.ToString();
                        
                        //  Play Fire Animation
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
                else
                {
                    Debug.Log("CLICK (you are out of ammo");
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
