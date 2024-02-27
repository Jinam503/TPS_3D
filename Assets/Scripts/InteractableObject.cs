using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected PlayerManager player;
    protected Collider interactableCollider;
    [SerializeField] GameObject interactableCanvus;

    private void OnTriggerEnter(Collider other)
    {
        if (!player)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player)
        {
            interactableCanvus.SetActive(true);
            player.canInteract = true; 
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (player)
        {
            if (player.inputManager.inputInteract)
            {
                Interact(player);
                player.inputManager.inputInteract = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!player)
        {
            player = other.GetComponent<PlayerManager>();
        }
        if (player)
        {
            interactableCanvus.SetActive(false);
            player.canInteract = false; 
        }
    }

    protected virtual void Interact(PlayerManager player)
    {
        Debug.Log("You have Interacted");
    }
}
