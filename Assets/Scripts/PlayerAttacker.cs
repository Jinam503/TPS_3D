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
                //  Camera Shake
                StartCoroutine(playerManager.cameraManager.GunRecoil());
                
                //  Minus Bullet from magazine
                playerManager.playerEquipment.CurrentWeapon.remainingAmmo--;
                playerManager.playerUIManager.currentAmmoCountText.text = playerManager.playerEquipment.CurrentWeapon.remainingAmmo.ToString();
                        
                //  Play Fire Animation
                playerManager.animatorManager.PlayTargetAnimation(weaponItem.Rifle_Fire, false);
                
                        
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
        BoxOfAmmoItem boxOfAmmoItem = playerManager.playerInventory.currentAmmoInInventory;

        if (currentWeapon.remainingAmmo == currentWeapon.maxAmmo)
        {
            Debug.Log("Ammo Already Full");
            return;
        }
        
        //  Check to see if it has ammo in inventory for this particular weapon, if we do not, return
        if (boxOfAmmoItem){
            if (boxOfAmmoItem.ammoType == currentWeapon.ammotype && boxOfAmmoItem.ammoRemaining > 0)
            {
                isReloading = true;
                rightHandRigWeight = 0f;
                aimRigWeight = 0f;
        
                playerManager.animatorManager.animator.SetBool("IsReloading", true);
                playerManager.animatorManager.PlayTargetAnimation("Rifle Reload", false);
            }
        }
    
    }

    public void EndReload()
    {
        //  Change Ammo States
        isReloading = false;
        WeaponItem currentWeapon = playerManager.playerEquipment.CurrentWeapon;
        BoxOfAmmoItem boxOfAmmoItem = playerManager.playerInventory.currentAmmoInInventory;
        
        int amountOfAmmoToReload =
            playerManager.playerEquipment.CurrentWeapon.maxAmmo -
            playerManager.playerEquipment.CurrentWeapon.remainingAmmo;

        //  If it has MORE ammo remaining than we need to put in our weapon, we subtract from TOTAL
        if (boxOfAmmoItem.ammoRemaining >= amountOfAmmoToReload)
        {
            currentWeapon.remainingAmmo = currentWeapon.maxAmmo;
            boxOfAmmoItem.ammoRemaining -= amountOfAmmoToReload;
        }
        //  If it has LESS ammo remaining than we need to put in our weapon, we just set to 0 and put in to our weapon
        else
        {
            currentWeapon.remainingAmmo += boxOfAmmoItem.ammoRemaining;
            boxOfAmmoItem.ammoRemaining = 0;
        }
                
        playerManager.playerUIManager.reservedAmmoCountText.text = boxOfAmmoItem.ammoRemaining.ToString();
        playerManager.playerUIManager.currentAmmoCountText.text = currentWeapon.remainingAmmo.ToString();
    }

    private void HandleAiming()
    {
        playerManager.animatorManager.animator.SetBool("IsAiming", isAiming);
    }

    private void HandleFiring()
    {
        playerManager.animatorManager.animator.SetBool("IsFiring", isFiring);
    }
}
