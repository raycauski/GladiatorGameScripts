using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    //private float fallingSpeed = 2.6f;
    //private float fallingAcceleration = 3f;

    public float maxSpeed = 4.5f;
    public float accelerationRate = 5f;

    public override void EnterState()
    {
        //playerStateMachine.SetCurrentMovement(fallingSpeed, fallingAcceleration);
    }
    public override void LogicUpdate()
    {
        // Check when controller hits ground to leave falling state
        if (PlayerMovement.IsGrounded())
        {
            StateMachine.ChangeState(StateMachine.PlayerRangedState);
        }
    }
    public override void ExitState()
    {

    }

    // TODO: Add falling attack transition


}
