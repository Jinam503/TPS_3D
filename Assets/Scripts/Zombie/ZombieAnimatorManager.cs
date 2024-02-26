using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimatorManager : MonoBehaviour
{
    private ZombieManager zombieManager;

    private void Awake()
    {
        zombieManager = GetComponent<ZombieManager>();
    }

    public void PlayTargetAttackAnimation(string attackAnimation)
    {
        zombieManager.animator.applyRootMotion = true;
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade(attackAnimation, 0.2f);
    }

    public void PlayTargetActionAnimation(string actionAnimation)
    {
        zombieManager.animator.applyRootMotion = true;
        zombieManager.isPerformingAction = true;
        zombieManager.animator.CrossFade(actionAnimation, 0.2f);
    }
}
