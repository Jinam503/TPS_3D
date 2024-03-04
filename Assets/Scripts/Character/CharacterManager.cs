using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;

    [Header("FLAGS")]
    public bool isGrounded;
    public bool isPerformingAction;
    public bool canRotate;
    public bool canMove;
    public bool isDead;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void LateUpdate()
    {
        
    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        characterStatsManager.currentHealth = 0;
        isDead = true;

        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Death From The Front", true, false,false);
        }

        yield return new WaitForSeconds(5);
    }
}
