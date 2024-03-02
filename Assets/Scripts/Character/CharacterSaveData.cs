using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("SavePoint")] 
    public string savePoint = "Save";
    
    [Header("TIme Played")]
    public float secondsPlayed;

    [Header("World Coodinates")] 
    public float xPos;
    public float yPos;
    public float zPos;

    [Header("Stats")] 
    public int health;
}
