using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    [SerializeField] protected CharacterManager character;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    
    public virtual void PlayTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool canMove = true,
        bool canRotate = true)
    {
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;
        
        character.animator.CrossFade(targetAnimation, 0.2f);
    }
}
