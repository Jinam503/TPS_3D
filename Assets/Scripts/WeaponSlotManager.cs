using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot handSlot;
    private PlayerUIManager playerUIManager;

    private void Awake()
    {
        handSlot = GetComponentInChildren<WeaponHolderSlot>();
        playerUIManager = GetComponentInParent<PlayerUIManager>();
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        handSlot.LoadWeaponModel(weaponItem);
        playerUIManager.currentAmmoCountText.text = weaponItem.remainingAmmo.ToString();
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
