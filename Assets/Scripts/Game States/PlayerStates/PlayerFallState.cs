using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    //private float fallingSpeed = 2.6f;
   // private float fallingAcceleration = 3f;


    public float maxSpeed = 4.5f;
    public float accelerationRate = 5f;

    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        //playerStateMachine.SetCurrentMovement(fallingSpeed, fallingAcceleration);
    }
    public override void LogicUpdate(PlayerStateMachine playerStateMachine)
    {
        // Check when controller hits ground to leave falling state
        if (playerStateMachine.IsGrounded())
        {
            playerStateMachine.ChangeState(playerStateMachine.playerRangedState);
        }
    }
    public override void ExitState(PlayerStateMachine playerStateMachine)
    {

    }


}
