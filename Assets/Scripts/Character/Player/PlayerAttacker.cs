using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerManager player;

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
    public float leftHandRigWeight;

    private float shootTimer;   
    public float shootTimerMax;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        leftHandRig.weight = Mathf.Lerp(leftHandRig.weight, leftHandRigWeight, Time.deltaTime * 20f);
    }

    public void HandleAiming()
    {
        if (isAiming)
        {
            if (!isReloading)
            {
                aimRigWeight = 1f;
                leftHandRigWeight = 1f;
            }
        }
        else
        {
            aimRigWeight = 0f;
        }

        if (player.playerLocomotion.isRunning)
        {
            leftHandRigWeight = 1f;
        }
        else
        {
            leftHandRigWeight = 0f;
        }
    }
    public void HandleFire(WeaponItem weaponItem)
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer > 0f || !isFiring) return;
        
        if (player.playerEquipment.CurrentWeapon.remainingAmmo > 0)
        {
            shootTimer = shootTimerMax;
                    
            //  When Aiming and player Alive
            if (isAiming) 
            {   
                //  Minus Bullet from magazine
                player.playerEquipment.CurrentWeapon.remainingAmmo--;
                PlayerUIManager.instance.currentAmmoCountText.text = player.playerEquipment.CurrentWeapon.remainingAmmo.ToString();
                        
                //  Play Fire Animation
                player.playerAnimatorManager.PlayTargetActionAnimation(weaponItem.Rifle_Fire, false);
                
                player.characterSoundFXManager.PlayFireSoundFX();
                        
                //  Spawn Muzzle Effect
                Vector3 muzzleSpawnPosition = weaponItem.muzzleSpawnPosition.position;
                        
                Transform muzzleFlash = 
                    Instantiate(
                        weaponItem.muzzleFlashPrefab,
                        muzzleSpawnPosition,
                        Quaternion.LookRotation(PlayerCamera.instance.cameraObject.transform.forward)
                        ).transform;
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
                        int damage = player.playerEquipment.weaponSlotManager.ReturnCurrentWeaponItemInHandSlot().damage;
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
        WeaponItem currentWeapon = player.playerEquipment.CurrentWeapon;
        int remainingAmmoInInventory = player.playerInventory.GetAmountOfAmmoByAmmoType(currentWeapon.ammotype);

        if (currentWeapon.remainingAmmo == currentWeapon.maxAmmo)
        {
            Debug.Log("Ammo Already Full");
            return;
        }
        
        if (remainingAmmoInInventory > 0)
        {
            isReloading = true;
            leftHandRigWeight = 0f;
            aimRigWeight = 0f;
        
            player.animator.SetBool("IsReloading", true);
            player.playerAnimatorManager.PlayTargetActionAnimation("Rifle Reload", false);
        }
    
    }
    public void EndReload()
    {
        //  Change Ammo States
        isReloading = false;
        WeaponItem currentWeapon = player.playerEquipment.CurrentWeapon;
        int remainingAmmoInInventory = player.playerInventory.GetAmountOfAmmoByAmmoType(currentWeapon.ammotype);
        
        int amountOfAmmoToReload =
            player.playerEquipment.CurrentWeapon.maxAmmo -
            player.playerEquipment.CurrentWeapon.remainingAmmo;
        
        currentWeapon.remainingAmmo += player.playerInventory.UseAmmoByAmmoType(currentWeapon, amountOfAmmoToReload);
        
        PlayerUIManager.instance.reservedAmmoCountText.text = 
            player.playerInventory.GetAmountOfAmmoByAmmoType(currentWeapon.ammotype).ToString();
        PlayerUIManager.instance.currentAmmoCountText.text = currentWeapon.remainingAmmo.ToString();
    }

}
