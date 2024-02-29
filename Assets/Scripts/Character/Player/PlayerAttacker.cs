using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerManager playerManager;

    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig leftHandRig;

    [Header("WeaponBulletRange")] 
    public float bulletRange = 100f;

    [Header("Shootable Layer")]
    public LayerMask shootableLayers;

    public bool isAiming;
    public bool isFiring;
    public bool isReloading;

    public float aimRigWeight;
    public float rightHandRigWeight;

    private float shootTimer;
    private float shootTimerMax = 0.2f;

    private void Awake()
    {
        playerManager = GetComponentInChildren<PlayerManager>();
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
        shootTimer -= Time.deltaTime;
        if (shootTimer > 0f || !isFiring) return;
        
        if (playerManager.playerEquipment.CurrentWeapon.remainingAmmo > 0)
        {
            isFiring = false;
            shootTimer = shootTimerMax;
                    
            //  When Aiming and player Alive
            if (isAiming && !playerManager.isDead) 
            {   
                //  Minus Bullet from magazine
                playerManager.playerEquipment.CurrentWeapon.remainingAmmo--;
                playerManager.playerUIManager.currentAmmoCountText.text = playerManager.playerEquipment.CurrentWeapon.remainingAmmo.ToString();
                        
                //  Play Fire Animation
                playerManager.playerAnimatorManager.PlayTargetActionAnimation(weaponItem.Rifle_Fire, false);
                
                        
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
                        int damage = playerManager.playerEquipment.weaponSlotManager.ReturnCurrentWeaponItemInHandSlot().damage;
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

    public void HandleReload()
    {
        WeaponItem currentWeapon = playerManager.playerEquipment.CurrentWeapon;
        int remainingAmmoInInventory = playerManager.playerInventory.GetAmountOfAmmoByAmmoType(currentWeapon.ammotype);

        if (currentWeapon.remainingAmmo == currentWeapon.maxAmmo)
        {
            Debug.Log("Ammo Already Full");
            return;
        }
        
        if (remainingAmmoInInventory > 0)
        {
            isReloading = true;
            rightHandRigWeight = 0f;
            aimRigWeight = 0f;
        
            playerManager.animator.SetBool("IsReloading", true);
            playerManager.playerAnimatorManager.PlayTargetActionAnimation("Rifle Reload", false);
        }
    
    }

    public void EndReload()
    {
        //  Change Ammo States
        isReloading = false;
        WeaponItem currentWeapon = playerManager.playerEquipment.CurrentWeapon;
        int remainingAmmoInInventory = playerManager.playerInventory.GetAmountOfAmmoByAmmoType(currentWeapon.ammotype);
        
        int amountOfAmmoToReload =
            playerManager.playerEquipment.CurrentWeapon.maxAmmo -
            playerManager.playerEquipment.CurrentWeapon.remainingAmmo;
        
        currentWeapon.remainingAmmo += playerManager.playerInventory.UseAmmoByAmmoType(currentWeapon, amountOfAmmoToReload);
        
        playerManager.playerUIManager.reservedAmmoCountText.text = 
            playerManager.playerInventory.GetAmountOfAmmoByAmmoType(currentWeapon.ammotype).ToString();
        playerManager.playerUIManager.currentAmmoCountText.text = currentWeapon.remainingAmmo.ToString();
    }

    private void HandleAiming()
    {
        playerManager.animator.SetBool("IsAiming", isAiming);
    }

    private void HandleFiring()
    {
        playerManager.animator.SetBool("IsFiring", isFiring);
    }
}
