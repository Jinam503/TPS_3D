using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("DAMAGE")]
    public int damage;

    [Header("CONTACT POINT")] 
    private Vector3 contactPoint;

    [Header("CHARACTERS DAMAGED")] 
    protected List<CharacterManager> characterDamaged = new List<CharacterManager>();
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        if (characterDamaged.Contains(damageTarget))
            return;

        characterDamaged.Add(damageTarget);
        
        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

        damageEffect.damage = damage;
        
        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }
}
