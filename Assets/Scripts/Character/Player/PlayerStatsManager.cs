using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        PlayerUIManager.instance.healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }
    protected override void Update()
    {
        base.Update();

        PlayerUIManager.instance.healthBar.SetCurrentHealth(currentHealth);
    }
}
