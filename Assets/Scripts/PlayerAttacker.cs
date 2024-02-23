using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    [SerializeField] private Rig aimRig;

    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform spawnBulletPosition;

    public bool isAiming;
    public bool isFiring;

    public float aimRigWeight;

    private float shootTimer;
    private float shootTimerMax = 0.1f;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Update()
    {
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
    }

    private void FixedUpdate()
    {
        HandleAiming();
        HandleFiring();
    }


    public void HandleFire(WeaponItem weaponItem)
    {
        if (isFiring)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                shootTimer += shootTimerMax + Random.Range(0f, shootTimerMax * 0.25f);

                if (isAiming && !playerLocomotion.isDied)
                {
                    animatorManager.PlayTargetAnimation(weaponItem.Rifle_Fire, false); // Play Fire Animation

                    Vector3 aimDir = (Mouse3D.GetMouseWorldPosition() - spawnBulletPosition.position).normalized;
                    Instantiate(bulletProjectilePrefab, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                }
            }
        }
    }

    private void HandleAiming()
    {
        animatorManager.animator.SetBool("IsAiming", isAiming);
    }

    private void HandleFiring()
    {
        animatorManager.animator.SetBool("IsFiring", isFiring);
    }
}
