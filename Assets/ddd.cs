using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ddd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        Vector3 playerVelocity = Vector3.zero;
        playerVelocity.y = 10;
        rig.velocity = playerVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
