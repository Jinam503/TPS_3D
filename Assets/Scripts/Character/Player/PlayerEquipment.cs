using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public WeaponSlotManager weaponSlotManager;

    public WeaponItem weaponItem;

    public WeaponItem CurrentWeapon => weaponSlotManager.ReturnCurrentWeaponItemInHandSlot();

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        //  Must be Deleted
        weaponSlotManager.LoadWeaponOnSlot(weaponItem);
    }
}
