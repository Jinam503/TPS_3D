using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("CHARACTER CAUSING DAMAGE")] 
    public CharacterManager characterCausingDamage;
    
    [Header("DAMAGE")]
    public int damage;

    [Header("POISE")] 
    public int poiseDamage;
    public bool poiseIsBroken = false;  

    [Header("ANIMATIONS")] 
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("SOUND FX")] 
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX;

    [Header("DIRECTION DAMAGE TAKEN FROM")]
    public float angleHitFrom;
    public Vector3 contactPoint;
    
    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (character.isDead)
            return;

        character.characterStatsManager.TakeDamage(damage);
    }
}
