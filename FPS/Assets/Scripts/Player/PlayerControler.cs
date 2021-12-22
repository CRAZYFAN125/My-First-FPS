using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControler : MonoBehaviour
{
    [Header("Set data")]
    public CharacterController controller;

    public float speed = 12f;
    Vector3 velocity;
    public float gravity = -10f;

    public Transform GroundCheker;
    public float groundDistance = 0.4f;
    public LayerMask GroundMask;
    bool isGrounded;
    public float jumpHeight = 2f;

    float x = 0;
    float z = 0;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheker.position, groundDistance, GroundMask);

        if (isGrounded&&velocity.y<0)
        {
            velocity.y = -2f;
        }

        
        
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 vector = context.ReadValue<Vector2>();
        x = vector.x; z = vector.y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (/*Input.GetButtonDown("Jump") && */isGrounded&&context.performed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundDistance > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GroundCheker.position, groundDistance);
        }
    }
}
