using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputManager playerInput;
    private CharacterController playerController;

    private float dashSpeed = 12f;
    private float dashDeceleration = 6f;

    private float dashMovementSpeed = 0f;
    private float dashAccelRate = 0f;

    private Vector2 dashDirection;
    public Vector3 dashVelocity { get; private set; } = Vector3.zero;
    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        playerInput = stateMachine.playerInput;
        playerController = stateMachine.playerController;
        CalculateDashTrajectory();
        
    }
    public override void LogicUpdate(PlayerStateMachine playerStateMachine)
    {
        DashMovement();
    }
    public override void ExitState(PlayerStateMachine playerStateMachine)
    {
        dashVelocity = Vector3.zero;
    }

    private void CalculateDashTrajectory()
    {
        if (playerInput.movementInput == Vector2.zero)
        {
            dashDirection = Vector2.down * dashSpeed; // if not pressing direction keys, automatically dodge backwards
        }
        else
        {
            dashDirection = playerInput.movementInput.normalized * dashSpeed; // otherwise dodge into movement input direction
        }
        dashVelocity = Quaternion.Euler(0, playerController.transform.eulerAngles.y, 0) *
            new Vector3(dashDirection.x, stateMachine.playerVelocity.y, dashDirection.y); // alligns direction with player facing direction
    }
    private void DashMovement()
    {
        // Creates a smooth dash that starts fast and slows to 0.
        dashVelocity = Vector3.Lerp(dashVelocity, new Vector3(0, stateMachine.playerVelocity.y, 0), dashDeceleration * Time.deltaTime);
        stateMachine.SetDashVelocity(dashVelocity);
    }
}
