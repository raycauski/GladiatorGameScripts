using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for ranged gameplay, where character can  move and aim.
public class PlayerRangedState : PlayerBaseState
{
    private PlayerInputManager playerInput;
    private GameObject playerCameraHolder;

    private PlayerStateMachine stateMachine;

    private float maxSpeed = 4.5f;
    private float accelerationRate = 6.2f; // 10f
    private float maxSpeedAiming;
    private float aimSpeedMultiplier;


    
    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        playerInput = playerStateMachine.playerInput;
        playerCameraHolder = playerStateMachine.playerCameraHolder;

        maxSpeedAiming = maxSpeed * aimSpeedMultiplier;

        playerStateMachine.SetCurrentMovement(maxSpeed, accelerationRate);
    }
    public override void LogicUpdate(PlayerStateMachine playerStateMachine)
    {
        CheckForSprint();
        CheckForCrouch();
    }
    public override void ExitState(PlayerStateMachine playerStateMachine)
    {
    
    }


    private void CheckForSprint()
    {
        if (playerInput.isSprinting && playerInput.movementInput != Vector2.zero)
        {
            Sprint();
        }
    }

    private void CheckForCrouch()
    {
        if (playerInput.isCrouching)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        stateMachine.ChangeState(stateMachine.playerCrouchState);
    }
    private void Sprint()
    {
        stateMachine.ChangeState(stateMachine.playerSprintState);
    }

}
