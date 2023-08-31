using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGunTransform : MonoBehaviour
{
    public PlayerLocomotion playerLocomotion;
    public GameObject gun;
    public Transform[] gunTransforms;
    public InputManager inputManager;

    private void Update()
    {
        if (!playerLocomotion.isGrounded)
        {
            gun.transform.position = Vector3.Lerp(gun.transform.position, gunTransforms[0].position, Time.deltaTime * 15);
            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, gunTransforms[0].rotation, Time.deltaTime * 15);
        }
        else
        {
            if (playerLocomotion.isSprinting && inputManager.moveAmount >= 0.5f)
            {
                gun.transform.position = Vector3.Lerp(gun.transform.position, gunTransforms[2].position, Time.deltaTime * 15);
                gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, gunTransforms[2].rotation, Time.deltaTime * 15);
            }
            else if (inputManager.moveAmount > 0f)
            {
                if (playerLocomotion.movementMode == false)
                {
                    gun.transform.position = Vector3.Lerp(gun.transform.position, gunTransforms[3].position, Time.deltaTime * 15);
                    gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, gunTransforms[3].rotation, Time.deltaTime * 15);
                }
                else
                {
                    gun.transform.position = Vector3.Lerp(gun.transform.position, gunTransforms[4].position, Time.deltaTime * 15);
                    gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, gunTransforms[4].rotation, Time.deltaTime * 15);
                }
            }
            else
            {
                gun.transform.position = Vector3.Lerp(gun.transform.position, gunTransforms[1].position, Time.deltaTime * 15);
                gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, gunTransforms[1].rotation, Time.deltaTime * 15);
            }
        }
    }
}
