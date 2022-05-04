using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerMulti player;
    [SerializeField] private float sensivity = 100f;
    [SerializeField] private float clampAngle = 80f;

    private float verticalRotation;
    private float horizontalRotation;

    private void OnValidate()
    {
        if (player==null)
        {
            player = GetComponentInParent<PlayerMulti>();
        }
    }

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }

        if (Cursor.lockState==CursorLockMode.Locked)
        {
            Look();
        }
    }
    private void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState==CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
        
    }
    private void Look()
    {
    Vector2 mouse = Vector2.zero;
        mouse.y = -Input.GetAxis("Mouse Y");
        mouse.x = -Input.GetAxis("Mouse X");

        verticalRotation = mouse.y*sensivity*Time.deltaTime;
        horizontalRotation = mouse.x*sensivity*Time.deltaTime;

        verticalRotation= Mathf.Clamp(verticalRotation,-clampAngle,clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }


}
