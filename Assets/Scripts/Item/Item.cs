using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;
    public Sprite itemIcon;
    public string itemID;
    
    public int maxquantity;
    public bool isStackable;
    
    [TextArea]
    public string description;
}
