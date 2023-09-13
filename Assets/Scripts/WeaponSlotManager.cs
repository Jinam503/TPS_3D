using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot handSlot;

    private void Awake()
    {
        handSlot = GetComponentInChildren<WeaponHolderSlot>();
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        handSlot.LoadWeaponModel(weaponItem);   
    }
}
