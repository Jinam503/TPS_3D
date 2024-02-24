using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    public Transform muzzleSpawnPosition; 
    public GameObject muzzleFlashPrefab;
    
    [Header("Rifle Firing animation")]
    public string Rifle_Fire;
    public string Rifle_Reload;
}
