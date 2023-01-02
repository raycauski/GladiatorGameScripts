using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private GameObject playerCameraHolder;
    private FPSCamera camFPS;
    private float maxSpeedCrouch = 2.5f;
    private float accelerationCrouch = 15f;

    public override void EnterState()
    {
        playerCameraHolder = PlayerMovement.playerCameraHolder;
        camFPS = playerCameraHolder.GetComponent<FPSCamera>();
 
        EnableCrouch();
    }
    public override void LogicUpdate()
    {
        CheckDisableCrouch();
    }
    public override void ExitState()
    {
        PlayerInput.isCrouching = false;
        camFPS.SetCrouching(false);
        PlayerMovement.playerHands.SetCrouchAnimation(false); // Anim
    }

    private void EnableCrouch()
    {

        PlayerMovement.SetCurrentMovement(maxSpeedCrouch, accelerationCrouch);
        camFPS.SetCrouching(true);
        PlayerMovement.playerHands.SetCrouchAnimation(true); // Anim
    }
    private void CheckDisableCrouch()
    {
        if (!PlayerInput.isCrouching)
        {
            StateMachine.ChangeState(StateMachine.playerRangedState);
        }
    }
}
