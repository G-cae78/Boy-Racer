using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    public Rigidbody rigid;               // Rigidbody for physics-based movement
    public float acceleration = 8000f;    // Force applied for forward/backward motion
    public float maxSpeed = 100f;          // Max speed of the car
    public float turnTorque = 500f;       // Torque applied for turning
    public float brakeForce = 0.2f;       // Brake strength (0 to 1)
    public float drag = 10f;               // Drag to simulate friction

    void Start()
    {
        // Initialize Rigidbody
        rigid = GetComponent<Rigidbody>();
        rigid.linearDamping = drag; // Set linear drag
        rigid.angularDamping = drag; // Set angular drag
        rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Ensure the Rigidbody doesn’t move in Y-axis
        rigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY;
    }

    void FixedUpdate()
    {
        // Freeze movement if the game is paused
    if (GameManagerScript.CurrentGameState == GameManagerScript.GAMESTATES.PAUSE)
    {
        rigid.linearVelocity = Vector3.zero;  // Stop all movement
        rigid.angularVelocity = Vector3.zero;  // Stop rotation
        return;  // Don't process input or movement when paused
    }
        // Get input
        float verticalInput = 0f;
        float horizontalInput = 0f;
        
        if (Input.GetKey(KeyCode.UpArrow)) verticalInput = 1f;    // Forward
        else if (Input.GetKey(KeyCode.DownArrow)){ 
            verticalInput = -1f; // Reverse
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f; // Left
        else if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f; // Right

        // Limit the car's forward speed
        if (rigid.linearVelocity.magnitude < maxSpeed)
        {
            rigid.AddForce(transform.forward * verticalInput*2 * acceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // Apply torque for turning
        if (verticalInput != 0) // Turn only while moving
        {
            rigid.AddTorque(Vector3.up * horizontalInput * turnTorque * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // Apply brakes if no input is given
        if (verticalInput == 0)
        {
            rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, Vector3.zero, brakeForce * Time.fixedDeltaTime);
            rigid.angularVelocity = Vector3.Lerp(rigid.angularVelocity, Vector3.zero, brakeForce * Time.fixedDeltaTime);
        }
    }
    }

