using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Box Of Ammo Item")]
public class BoxOfAmmoItem : Item
{
    public AmmoType ammoType;
    public int ammoRemaining = 30;
    public int boxOfAmmoCapacity = 30;
}
