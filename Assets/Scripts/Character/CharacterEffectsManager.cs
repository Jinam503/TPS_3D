using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    //  INSTANT EFFECTS ( TAKE DAMAGE, HEAL)
    //  TIMED EFFECTS (POISON)
    //  STATIC EFFECTS 

    private CharacterManager character;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }
}
