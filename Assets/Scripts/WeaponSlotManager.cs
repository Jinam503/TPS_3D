using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot handSlot;
    private PlayerManager playerManager;

    private void Awake()
    {
        handSlot = GetComponentInChildren<WeaponHolderSlot>();
        playerManager = GetComponentInParent<PlayerManager>();
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        handSlot.LoadWeaponModel(weaponItem);
        playerManager.playerEquipment.CurrentWeapon.remainingAmmo = playerManager.playerEquipment.CurrentWeapon.maxAmmo;
        playerManager.playerUIManager.currentAmmoCountText.text = weaponItem.remainingAmmo.ToString();

        if (playerManager.playerInventory.currentAmmoInInventory != null)
        {   
            if (playerManager.playerInventory.currentAmmoInInventory.ammoType == weaponItem.ammotype)
            {
                playerManager.playerUIManager.reservedAmmoCountText.text =
                    playerManager.playerInventory.currentAmmoInInventory.ammoRemaining.ToString();
            }
        }
    }

    public WeaponItem ReturnCurrentWeaponItemInHandSlot()
    {
        if (handSlot.currentWeapon)
        {
            return handSlot.currentWeapon;
        }
        else
        {
            return null;
        }
    }
}
