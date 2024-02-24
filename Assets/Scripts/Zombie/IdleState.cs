using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private PersueTargetState persueTargetState;
    
    //  The layer used to detect potential attack targets
    [Header("Detection Layer")]
    [SerializeField] private LayerMask detectionLayer;

    [Header("Line Of Sight Detection")]
    [SerializeField] private float characterEyeLevel;
    [SerializeField] private LayerMask ignoreForLineOfSightDetection;

    //  How far it can detect a target
    [Header("Detection Radius")]
    [SerializeField] private float detectionRadius = 5;
    
    //  how wide it can see a target within our FIELD OF VIEW
    [Header("Detection Angle Radius")]
    [SerializeField] private float minimumDetectionRadiusAngle = -35f;
    [SerializeField] private float maximumDetectionRadiusAngle = 35f;

    private void Awake()
    {
        persueTargetState = GetComponent<PersueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            return persueTargetState;
        }
        else
        {
            FindTargetViaLineOfSight(zombieManager);
            return this;
        }
    }

    private void FindTargetViaLineOfSight(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        
        Debug.Log("Checking for Colliders");

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();
            
            //  if the playerManager is detected, we then check for line of sight
            if (player != null)
            {
                Debug.Log("found the player collider");
                Vector3 targetDirection = transform.position - player.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > minimumDetectionRadiusAngle && viewableAngle < maximumDetectionRadiusAngle)
                {
                    Debug.Log("We have passed the field of view check");
                    RaycastHit hit;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x,characterEyeLevel, player.transform.position.z );
                    Vector3 zombieStartPoint = new Vector3(transform.position.x,characterEyeLevel, transform.position.z );
                    
                    Debug.DrawLine(playerStartPoint,zombieStartPoint, Color.cyan);
                    //  Check one last time for object blocking view
                    if (Physics.Linecast(playerStartPoint, zombieStartPoint, out hit,ignoreForLineOfSightDetection))
                    {
                        Debug.Log("There is something in the way");
                    }
                    else
                    {
                        Debug.Log("We have a target, switching states");
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }
}
