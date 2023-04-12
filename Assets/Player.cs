using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float mX;
    private float mY;
    private float h;
    private float v;

    public Transform cameraArm;
    public float moveSpeed;

    public Animator anim;

    void Start()
    {
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Inputs();
        CameraMove();
        Move();
    }
    private void Inputs()
    {
        mX = Input.GetAxis("Mouse X");
        mY = Input.GetAxis("Mouse Y");
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }
    private void CameraMove()
    {
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        Vector3 playerAngle = transform.rotation.eulerAngles;
        float x = camAngle.x - mY;
        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 300f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y, camAngle.z);
        transform.rotation = Quaternion.Euler(0, playerAngle.y + mX, 0);
    }
    private void Move()
    {
        Vector3 foward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;

        transform.forward = foward;

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        if (h != 0 || v != 0)
        {
            anim.SetBool("IsMoving",true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }
}
