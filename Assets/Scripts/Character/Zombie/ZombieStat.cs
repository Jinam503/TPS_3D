using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStat : MonoBehaviour
{
    private ZombieManager zombie;
    
    [Header("Damage Modifiers")] 
    public float headShotDamageMultiplier = 1.5f;
    //
    
    [Header("Overall Health")]
    public int overallHealth = 100;
    
    [Header("Head Health")]
    public int headHealth = 100;
    
    [Header("UpperBodyHealth")]
    public int torsoHealth = 100;
    public int leftArmHealth = 100;
    public int rightArmHealth = 100;

    [Header("Lower Body Health")] 
    public int leftLegHealth = 100;
    public int rightLegHealth = 100;

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DealHeadShotDamage(int damage)
    {
        headHealth = headHealth - Mathf.RoundToInt(damage * headShotDamageMultiplier);
        overallHealth = overallHealth - Mathf.RoundToInt(damage * headShotDamageMultiplier);

        CheckForHealth();
    }
    public void DealTorsoDamage(int damage)
    {
        torsoHealth = torsoHealth - damage;
        overallHealth = overallHealth - damage;

        CheckForHealth();
    }
    public void DealArmDamage(bool leftArmDamage, int damage)
    {
        if (leftArmDamage)
        {
            if (leftArmHealth <= 0)
            {
                overallHealth -= damage;
            }
            else
            {
                leftArmHealth -= damage;
            }
        }
        else
        {
            if (rightArmHealth <= 0)
            {
                overallHealth -= damage;
            }
            else
            {
                rightArmHealth -= damage;
            }
        }

        CheckForHealth();
    }
    public void DealLegDamage(bool leftLegDamage, int damage)
    {
        if (leftLegDamage)
        {
            if (leftLegHealth <= 0)
            {
                overallHealth -= damage;
            }
            else
            {
                leftLegHealth -= damage;
            }
        }
        else
        {
            if (rightLegHealth <= 0)
            {
                overallHealth -= damage;
            }
            else
            {
                rightLegHealth -= damage;
            }
        }

        CheckForHealth();
    }
    private void CheckForHealth()
    {
        if (overallHealth <= 0)
        {
            overallHealth = 0;
            zombie.isDead = true;
            zombie.zombieAnimatorManager.PlayTargetActionAnimation("Zombie Dead", true);
        }
    }
}
