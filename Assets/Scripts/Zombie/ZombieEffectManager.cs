using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEffectManager : MonoBehaviour
{
    private ZombieManager zombie;
    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DamageZombieHead(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Foward Head", 0.2f);
        zombie.zombieStat.DealHeadShotDamage(damage);
    }

    public void DamageZombieTorso(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Foward Torso", 0.2f);  
        zombie.zombieStat.DealTorsoDamage(damage);
    }

    public void DamageZombieRightArm(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Foward Torso", 0.2f);  
        zombie.zombieStat.DealArmDamage(false, damage);
    }
    
    public void DamageZombieLeftArm(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Foward Torso", 0.2f);  
        zombie.zombieStat.DealArmDamage(true, damage);
    }
    
    public void DamageZombieLeftLeg(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Foward Torso", 0.2f);  
        zombie.zombieStat.DealLegDamage(true, damage);
    }
    
    public void DamageZombieRighLeg(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Foward Torso", 0.2f);  
        zombie.zombieStat.DealLegDamage(false, damage);
    }
}
