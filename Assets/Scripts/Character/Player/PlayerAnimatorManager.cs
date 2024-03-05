using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    private PlayerManager player;
    
    private int vertical;
    private int horizontal;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
        
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

        if (player.playerInventory.isInventoryOpened)
        {
            verticalAmount = 0;
            horizontalAmount = 0;
        }
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);   
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);   
    }
}
