using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputManager playerInput;

    private float sprintSpeed = 8f;
    private float sprintAccelerationRate = 5f;
    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        playerInput = stateMachine.playerInput;
        EnableSprint();
    }
    public override void LogicUpdate(PlayerStateMachine playerStateMachine)
    {
        CheckDisableSprint();
    }
    public override void ExitState(PlayerStateMachine playerStateMachine)
    {

    }

    private void EnableSprint()
    {
        stateMachine.SetCurrentMovement(sprintSpeed, sprintAccelerationRate);
    }

    private void CheckDisableSprint()
    {
        if (!playerInput.isSprinting || playerInput.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.playerRangedState);
        }
    }

}
