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

        playerManager.playerAnimatorManager.PlayTargetActionAnimation("Hit Reaction", false);

        if(currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        playerManager.playerAnimatorManager.PlayTargetActionAnimation("Death From The Front", true);
        playerManager.isDead = true;
    }
}
