using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    [SerializeField] private float dashTime = 0.25f;
    [SerializeField] private float dashSpeed = 12f;



    private Vector2 dashDirection;
    public override void EnterState()
    {
        CalculateDashTrajectory();
    }
    public override void LogicUpdate()
    {
        if (!PlayerMovement.isDashing)
        {
            StateMachine.ChangeState(StateMachine.PlayerRangedState);
        }
    }
    public override void ExitState()
    {
        
    }

    private void CalculateDashTrajectory()
    {
        if (PlayerInput.movementInput == Vector2.zero)
        {
            //dashDirection = Vector2.down * dashSpeed; // if not pressing direction keys, automatically dodge backwards
            dashDirection = Vector2.up * dashSpeed; // if not pressing direction keys, automatically dodge forwards
        }
        else
        {
            dashDirection = PlayerInput.movementInput.normalized * dashSpeed; // otherwise dodge into movement input direction
        }

        PlayerMovement.StartCoroutine(PlayerMovement.PerformDash(dashDirection, dashTime));

 
    }
}
