using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float fallingSpeed = 3f;
    private float fallingAcceleration = 5f;


    public float maxSpeed = 4.5f;
    public float accelerationRate = 5f;

    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        Debug.Log("Falling");
        playerStateMachine.SetCurrentMovement(fallingSpeed, fallingAcceleration);
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
        Debug.Log("Landed");
    }


}
