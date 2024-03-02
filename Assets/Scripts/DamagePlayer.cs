using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        PlayerStatsManager playerStatsManager = other.GetComponent<PlayerStatsManager>();

        if(playerStatsManager != null)
        {
            playerStatsManager.TakeDamage(damage);
        }
    }
}
