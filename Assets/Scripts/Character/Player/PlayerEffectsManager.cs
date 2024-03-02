using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug Delete Later")] 
    [SerializeField] private InstantCharacterEffect effectToTest;
    [SerializeField] private bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            InstantCharacterEffect effect = Instantiate(effectToTest);
            
            ProcessInstantEffect(effect);
        }
    }
}
