using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{

    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashDeceleration = 1f;
    [SerializeField] private float dashTime = 0.5f;


    private Vector2 dashDirection;
    public Vector3 dashVelocity { get; private set; } = Vector3.zero;
    public override void EnterState()
    {
        CalculateDashTrajectory();
    }
    public override void LogicUpdate()
    {
        DashMovement();
    }
    public override void ExitState()
    {
        dashVelocity = Vector3.zero;
    }

    private void CalculateDashTrajectory()
    {
        if (PlayerInput.movementInput == Vector2.zero)
        {
            dashDirection = Vector2.down * dashSpeed; // if not pressing direction keys, automatically dodge backwards
        }
        else
        {
            dashDirection = PlayerInput.movementInput.normalized * dashSpeed; // otherwise dodge into movement input direction
        }


        /*
        dashVelocity = Quaternion.Euler(0, playerController.transform.eulerAngles.y, 0) *
            new Vector3(dashDirection.x, stateMachine.playerVelocity.y, dashDirection.y); // alligns direction with player facing direction
        */
        StartCoroutine(DashTimer());    // Starts Dash coroutine
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashTime);
        StateMachine.ChangeState(StateMachine.playerRangedState);
    }

    public void SetDashVelocity(Vector3 newVelocity)
    {
       //PlayerMovement.SetCurrentMovement(0, 0);
    }
    private void DashMovement()
    {
        // Creates a smooth dash that starts fast and slows to 0.
        dashVelocity = Vector3.Lerp(dashVelocity, new Vector3(0, PlayerMovement.playerVelocity.y, 0), dashDeceleration * Time.deltaTime);
        SetDashVelocity(dashVelocity);
    }
}
