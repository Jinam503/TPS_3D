using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public GameObject currentWeaponModel;

    public PlayerLocomotion playerLocomotion;
    public Transform[] gunTransforms;
    public InputManager inputManager;

    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);     
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if(currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        if (weaponItem == null)
        {
            UnloadWeapon();

            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        if(model != null)
        {
            if(parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }
        currentWeaponModel = model;
    }

    private void Update()
    {
        if (currentWeaponModel == null || playerLocomotion.isDied)
            return;
        if (!playerLocomotion.isGrounded) // jumping
        {
            currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[0].position, Time.deltaTime * 15);
            currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[0].rotation, Time.deltaTime * 15);
        }
        else
        {
            if (playerLocomotion.isSprinting && inputManager.moveAmount >= 0.5f) // Sprinting
            {
                currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[2].position, Time.deltaTime * 15);
                currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[2].rotation, Time.deltaTime * 15);
            }
            else if (inputManager.moveAmount > 0f)
            {
                if (playerLocomotion.movementMode == false) // Walk
                {
                    if (playerLocomotion.isAiming)
                    {
                        currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[5].position, Time.deltaTime * 15);
                        currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[5].rotation, Time.deltaTime * 15);
                    }
                    else
                    {
                        currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[3].position, Time.deltaTime * 15);
                        currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[3].rotation, Time.deltaTime * 15);
                    }
                }
                else // Running
                {
                    currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[4].position, Time.deltaTime * 15);
                    currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[4].rotation, Time.deltaTime * 15);
                }
            }
            else // Idle
            {
                if (playerLocomotion.isAiming)
                {
                    currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[5].position, Time.deltaTime * 15);
                    currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[5].rotation, Time.deltaTime * 15);
                }
                else
                {
                    currentWeaponModel.transform.position = Vector3.Lerp(currentWeaponModel.transform.position, gunTransforms[1].position, Time.deltaTime * 15);
                    currentWeaponModel.transform.rotation = Quaternion.Slerp(currentWeaponModel.transform.rotation, gunTransforms[1].rotation, Time.deltaTime * 15);
                }
            }
        }
    }
}
