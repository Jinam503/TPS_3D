using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    protected CharacterManager character;

    private int vertical;
    private int horizontal;

    protected virtual void Awake()
    {
        character = GetComponentInParent<CharacterManager>();
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

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction)
    {
        character.isPerformingAction = isPerformingAction;
        character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation);
    }
}
