using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for ranged gameplay, where character can  move and aim.
public class PlayerRangedState : PlayerBaseState
{
    private CharacterController controller;
    private PlayerInputManager playerInput;
    private GameObject playerCameraHolder;
    private PlayerStateMachine stateMachine;

    private float maxSpeed = 4.5f;
    private float accelerationRate = 10f;
    private float maxSpeedAiming;
    private float aimSpeedMultiplier;
    
    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        controller = playerStateMachine.playerController;
        playerInput = playerStateMachine.playerInput;
        playerCameraHolder = playerStateMachine.playerCameraHolder;

        maxSpeedAiming = maxSpeed * aimSpeedMultiplier;

        playerStateMachine.SetCurrentMovement(maxSpeed, accelerationRate);
    }
    public override void LogicUpdate(PlayerStateMachine playerStateMachine)
    {
        CheckForSprint();
    }
    public override void ExitState(PlayerStateMachine playerStateMachine)
    {
        
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

    private void EnterDashState()
    {

    }

    private void CheckForSprint()
    {
        if (playerInput.isSprinting && playerInput.movementInput != Vector2.zero)
        {
            Sprint();
        }
    }
    private void Sprint()
    {
        stateMachine.ChangeState(stateMachine.playerSprintState);
    }

}
