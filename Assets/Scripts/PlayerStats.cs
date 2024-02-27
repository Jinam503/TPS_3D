using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerManager playerManager;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public HealthBar healthBar;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
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

        playerManager.animatorManager.PlayTargetAnimation("Hit Reaction", false);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            playerManager.animatorManager.PlayTargetAnimation("Death From The Front", true);
            playerManager.playerLocomotion.isDied = true;
        }
    }
}
