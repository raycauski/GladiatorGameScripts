using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    [SerializeField] private float dashTime = 0.25f;
    [SerializeField] private float dashSpeed = 12f;



    private Vector2 dashDirection;
    public Vector3 dashVelocity { get; private set; } = Vector3.zero;
    public override void EnterState()
    {
        CalculateDashTrajectory();
    }
    public override void LogicUpdate()
    {
        if (!PlayerMovement.isDashing)
        {
            StateMachine.ChangeState(StateMachine.playerRangedState);
        }
    }
    public override void ExitState()
    {
        dashVelocity = Vector3.zero;
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

        /*
        dashVelocity = Quaternion.Euler(0, playerController.transform.eulerAngles.y, 0) *
            new Vector3(dashDirection.x, stateMachine.playerVelocity.y, dashDirection.y); // alligns direction with player facing direction
        */
        //StartCoroutine(DashTimer());    // Starts Dash coroutine
    }

    /*
    private IEnumerator DashTimer()
    {
        yield return new WaitFor(dashTime);
        StateMachine.ChangeState(StateMachine.playerRangedState);
    }
    

    private void DashMovement()
    {
        // Creates a smooth dash that starts fast and slows to 0.
        dashVelocity = Vector3.Lerp(dashVelocity, new Vector3(0, PlayerMovement.playerVelocity.y, 0), dashDeceleration * Time.deltaTime);
    }
    */
}
