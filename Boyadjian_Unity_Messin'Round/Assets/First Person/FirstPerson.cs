using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public float gravity = -9.8f;
    public float jump_force = 10f;

    private CharacterController character_controller;
    private Vector2 move_input;
    private Vector3 player_velocity;
    private bool grounded;
    private Vector2 mouse_movement;

    public Camera camera;
    public float sensitivity = 30;
    private float cam_x_rotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        character_controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = character_controller.isGrounded;
        MovePlayer();
        Look();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
        //Debug.Log("Move Input: " + move_input.ToString());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Pressed the jump key");
        Jump();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouse_movement = context.ReadValue<Vector2>();
        //Debug.Log("Mouse Input: " + mouse_movement.ToString());
    }

    public void MovePlayer()
    {
        Vector3 move_vec = transform.right * move_input.x + transform.forward * move_input.y;
        character_controller.Move(move_vec * speed * Time.deltaTime);

        player_velocity.y += gravity * Time.deltaTime;
        if(grounded && player_velocity.y < 2)
        {
            player_velocity.y = -2;
        }
        character_controller.Move(player_velocity * Time.deltaTime);
    }

    public void Look()
    {
        float x_amount = mouse_movement.y * sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouse_movement.x * sensitivity * Time.deltaTime);

        cam_x_rotation -= x_amount;
        cam_x_rotation = Mathf.Clamp(cam_x_rotation, -90, 90);


        camera.transform.localRotation = Quaternion.Euler(cam_x_rotation, 0, 0);
    }

    public void Jump()
    {
        if(grounded)
        {
            player_velocity.y = jump_force;
        }
    }
}
