using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private PlayerStateMachine stateMachine;
    private PlayerInputManager playerInput;
    private GameObject playerCameraHolder;
    private FPSCamera camFPS;
    private float maxSpeedCrouch = 2.5f;
    private float accelerationCrouch = 15f;


    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        playerInput = stateMachine.playerInput;
        playerCameraHolder = playerStateMachine.playerCameraHolder;
        camFPS = playerCameraHolder.GetComponent<FPSCamera>();
 
        EnableCrouch();
    }
    public override void LogicUpdate()
    {
        CheckDisableCrouch();
    }
    public override void ExitState()
    {
        playerInput.isCrouching = false;
        camFPS.SetCrouching(false);
        stateMachine.playerHands.SetCrouchAnimation(false); // Anim
    }

    private void EnableCrouch()
    {

        stateMachine.SetCurrentMovement(maxSpeedCrouch, accelerationCrouch);
        camFPS.SetCrouching(true);
        stateMachine.playerHands.SetCrouchAnimation(true); // Anim
    }
    private void CheckDisableCrouch()
    {
        if (!playerInput.isCrouching)
        {
            stateMachine.ChangeState(stateMachine.playerRangedState);
        }
    }
}
