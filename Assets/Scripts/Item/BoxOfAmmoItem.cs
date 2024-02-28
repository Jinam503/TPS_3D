using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName ="Items/Box Of Ammo Item")]
public class BoxOfAmmoItem : Item
{
    public AmmoType ammoType;
    public int ammoQuantity;

    public int width;
    public int height;
}
