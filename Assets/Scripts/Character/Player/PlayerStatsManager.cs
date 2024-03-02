using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    public UI_HealthBar healthBar;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }
    protected override void Update()
    {
        base.Update();
        
        healthBar.SetCurrentHealth(currentHealth);
    }
}
