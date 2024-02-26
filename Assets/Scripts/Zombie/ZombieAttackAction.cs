using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Zombie Attack Action")]
public class ZombieAttackAction : ScriptableObject
{
    [Header("Attack Animation")] 
    public string attackAnimation;

    [Header("Attack Cooldown")] 
    public float attackCooldown = 5f;

    [Header("Attack Angles & Distances")] 
    public float maximumAttackAngle = 20f;
    public float minimumAttackAngle = -20f;
    public float maximumAttackDistance = 1f;
    public float minimumAttackDistance = 3.5f;
}
