using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public CharacterManager character;
    
    public int maxHealth;
    public int currentHealth;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //healthBar.SetCurrentHealth(currentHealth);
//
        //character.playerAnimatorManager.PlayTargetActionAnimation("Hit Reaction", false);

        if(currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        StartCoroutine(character.ProcessDeathEvent());
    }
}
