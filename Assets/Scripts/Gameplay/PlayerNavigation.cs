using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    private CharacterController controller;
    public Transform cameraPos;
    private Vector3 playerVelocity;

    
    private float velX = 0;
    private float velZ = 0;
    private float gravity = 9.8f;
  
    public float maxSpeed = 9.0f;
    private float maxSpeedAiming;
    public float accelerationRate = 25f;
    public float rotationSpeed = 2.0f;
    public bool isMoving;
    
    //[HideInInspector]
    public bool isAiming;
    

    public bool playerGrounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockCursor();
        maxSpeedAiming = maxSpeed * 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity(Time.deltaTime);
        HandleMovement(Time.deltaTime);
    }

    private void HandleMovement(float delta)
    {
        // Input Vectors
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        //Ismoving check and face camera while moving
        float curMaxSpeed;

        if (movementInput == Vector3.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (isMoving || isAiming)
        {
            RotatePlayer(Time.deltaTime);
        }
        if (isAiming)
        {
            curMaxSpeed = maxSpeedAiming;
        }
        else
        {
            curMaxSpeed = maxSpeed;
        }
        // Rotates direction of Player and calculates Accel/Deccel
        
        Vector3 targetVelocity = movementInput * curMaxSpeed;
        velX = Mathf.Lerp(velX, targetVelocity.x, Time.deltaTime * accelerationRate);
        velZ = Mathf.Lerp(velZ, targetVelocity.z, Time.deltaTime * accelerationRate);

        Vector3 currentVelocity = Quaternion.Euler(0, controller.transform.eulerAngles.y, 0) * new Vector3(velX, playerVelocity.y, velZ);

        // Apply Movement per frame
        controller.Move(currentVelocity * Time.deltaTime);

    }
    private void ApplyGravity(float delta)
    {
        // Grounded Check
        playerGrounded = controller.isGrounded;
        if (playerGrounded)
        {
            playerVelocity.y = 0;
        }
        //Apply Gravity
        playerVelocity.y -= gravity * delta;

    }

    public void RotatePlayer(float delta)
    {
        float targetRotation = Mathf.LerpAngle(transform.eulerAngles.y, cameraPos.eulerAngles.y, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, targetRotation, 0);
    }
    void LockCursor()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnlockCursor()
    {
        if (Input.GetButton("Escape"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
  
}
