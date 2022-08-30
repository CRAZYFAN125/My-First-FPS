using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMulti : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform cam;
    public float Sensitivity = 100f;

    private float xRotation = 0f;

    float mouseX = 0;
    float mouseY = 0;


    private void Start()
    {
        if (!isLocalPlayer)
        {
            transform.GetChild(0).GetComponent<Camera>().enabled=false;
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            rb = GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
        if (PlayerPrefs.GetFloat("Sensi") != 0)
        {
            Sensitivity = PlayerPrefs.GetFloat("Sensi");
        }
        }
    }

    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        MoveObject();
        RotateObject();
    }
    
    void RotateObject()
    {
        mouseX = -Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -85, 90);

        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);

    }

    void MoveObject()
    {
        Vector2 movePos = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movePos.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movePos.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movePos.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movePos.x = -1;
        }

        Vector3 move = transform.right * movePos.x + transform.forward * movePos.y;
        rb.MovePosition(transform.position + (move * Time.deltaTime) * 10);
    }
}
