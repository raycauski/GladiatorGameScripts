using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{

    private float sprintSpeed = 8f;
    private float sprintAccelerationRate = 5f;
    private float jumpSpeed = 6.5f;
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
        //PlayerInput.ResetStates();
        PlayerMovement.playerHands.SetSprintAnimation(false); // Anim
    }

    private void EnableSprint()
    {
        PlayerMovement.SetCurrentMovement(sprintSpeed, sprintAccelerationRate);
        PlayerMovement.playerHands.SetSprintAnimation(true); // Anim
    }

    private void CheckDisableSprint()
    {
        // Not sprinting Sprinting
        if (!PlayerInput.sprint.IsPressed() || PlayerInput.movementInput == Vector2.zero)
        {
            StateMachine.ChangeState(StateMachine.playerRangedState);
        }

        // Dash during sprint = Jump
        else if (PlayerInput.dash.triggered)
        {
            // Jump
            PlayerMovement.playerVelocity.y += jumpSpeed;
            StateMachine.ChangeState(StateMachine.playerDashState);
        }

        // Attack during sprint = running attack
        else if (PlayerInput.attack.triggered)
        {
            StateMachine.ChangeState(StateMachine.playerRangedState);
            StateMachine.playerRangedState.GetComponent<PlayerRangedState>().Attack();
        }
    }

}
