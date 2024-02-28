using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItemInteractable : InteractableObject
{
    public BoxOfAmmoItem boxOfAmmoItem;
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    protected override void Interact(PlayerManager player)
    {
        base.Interact(player);
        
        player.playerInventory.AddItem(boxOfAmmoItem, boxOfAmmoItem.ammoQuantity, boxOfAmmoItem.width, boxOfAmmoItem.height);
        
        Destroy(gameObject);
    }
}
