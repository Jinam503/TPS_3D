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
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
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
        Vector3 right = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 dir = foward * v + right * h;

        transform.forward = foward;
        transform.position += dir * Time.deltaTime * moveSpeed;
    }
}
