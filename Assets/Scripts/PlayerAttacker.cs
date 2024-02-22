using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void HandleFire(WeaponItem weaponItem)
    {
        if (playerLocomotion.isAiming && !playerLocomotion.isFiring && !playerLocomotion.isDied)
        {
            playerLocomotion.isFiring = true;
            animatorManager.PlayTargetAnimation(weaponItem.Rifle_Fire, false);
        }
    }
}
