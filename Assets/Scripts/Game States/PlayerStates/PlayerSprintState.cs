using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{

    private float sprintSpeed = 8f;
    private float sprintAccelerationRate = 5f;
    public override void EnterState()
    {
        EnableSprint();
    }
    public override void LogicUpdate()
    {
        CheckDisableSprint();
    }
    public override void ExitState()
    {
        PlayerMovement.playerHands.SetSprintAnimation(false); // Anim
    }

    private void EnableSprint()
    {
        PlayerMovement.SetCurrentMovement(sprintSpeed, sprintAccelerationRate);
        PlayerMovement.playerHands.SetSprintAnimation(true); // Anim
    }

    private void CheckDisableSprint()
    {
        if (!PlayerInput.isSprinting || PlayerInput.movementInput == Vector2.zero)
        {
            StateMachine.ChangeState(StateMachine.playerRangedState);
        }
    }

}
