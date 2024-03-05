using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimatorManager : CharacterAnimatorManager
{
    private ZombieManager zombieManager;

    protected override void Awake()
    {
        base.Awake();
        zombieManager = GetComponent<ZombieManager>();
    }

    
}
