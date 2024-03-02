using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    private PlayerManager player;
    
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public UI_HealthBar healthBar;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;

        //healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;

        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetCurrentHealth(currentHealth);

        player.playerAnimatorManager.PlayTargetActionAnimation("Hit Reaction", false);

        if(currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        player.playerAnimatorManager.PlayTargetActionAnimation("Death From The Front", true, false,false);
        player.isDead = true;
    }
}
