using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;

    int horizontal;
    int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting, bool movementMode)
    {
        if(verticalMovement > 0.5f)
        {
            if (movementMode)
            {
                verticalMovement = 1f;
            }
            else
            {
                verticalMovement = 0.5f;
            }
            if (isSprinting)
            {
                verticalMovement = 2;
            }
        }
        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
    }
}
