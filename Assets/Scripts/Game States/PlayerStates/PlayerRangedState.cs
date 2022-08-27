using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for ranged gameplay, where character can  move and aim.
public class PlayerRangedState : PlayerBaseState
{
    private CharacterController controller;
    private PlayerInputManager playerInput;
    private GameObject playerCameraHolder;
   
    private float velX = 0;
    private float velZ = 0;
    private float gravity = 12f;
    private Vector3 playerVelocity;

    public float maxSpeed = 4.5f;
    private float maxSpeedAiming;
    private float aimSpeedMultiplier;
    public float accelerationRate = 10f;
    public bool isMoving;

    public bool playerGrounded;
    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        controller = playerStateMachine.playerController;
        playerInput = playerStateMachine.playerInput;
        playerCameraHolder = playerStateMachine.playerCameraHolder;

        maxSpeedAiming = maxSpeed * aimSpeedMultiplier;
    }
    public override void LogicUpdate(PlayerStateMachine playerStateMachine)
    {
        ApplyGravity();
        HandleMovement();
    }
    public override void ExitState(PlayerStateMachine playerStateMachine)
    {
       
    }

    // =============================================================================
    private void HandleMovement()
    {
        // Input Vectors
        Vector2 movementInput = (playerInput.movementInput).normalized;
        //Ismoving check and face camera while moving
        float curMaxSpeed;

        // Slows movement while aiming ------- maybe later change to seperate player state?
        if (IsAiming())
        {
            curMaxSpeed = maxSpeedAiming;
            
        }
        else
        {
            curMaxSpeed = maxSpeed;
        }
        // Rotates direction of Player and calculates Accel/Deccel

        Vector2 targetVelocity = movementInput * curMaxSpeed;
        velX = Mathf.Lerp(velX, targetVelocity.x, Time.deltaTime * accelerationRate);
        velZ = Mathf.Lerp(velZ, targetVelocity.y, Time.deltaTime * accelerationRate);

        Vector3 currentVelocity = Quaternion.Euler(0, controller.transform.eulerAngles.y, 0) * new Vector3(velX, playerVelocity.y, velZ);

        // Apply Movement per frame
        controller.Move(currentVelocity * Time.deltaTime);

    }
    private void ApplyGravity()
    {
        // Grounded Check
        playerGrounded = controller.isGrounded;
        if (playerGrounded)
        {
            playerVelocity.y = 0;
        }
        //Apply Gravity
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    private bool IsAiming()
    {
        // Checks if player is aiming
        if (playerInput.isAiming)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
