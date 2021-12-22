using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float Sensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    float mouseX = 0;
    float mouseY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = mouseX * Sensitivity * Time.deltaTime;
        mouseY = mouseY * Sensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -85, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX);

    }
    public void LookAround(InputAction.CallbackContext context)
    {
        Vector2 vector = context.ReadValue<Vector2>();
        mouseX = vector.x;
        mouseY = vector.y;
    }
}
