using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    protected CharacterManager character;

    private int vertical;
    private int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isRunning)
    {
        float horizontalAmount = horizontalValue;
        float verticalAmount = verticalValue;
        if (isRunning)
        {
            verticalAmount = 2;
        }
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);   
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);   
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
