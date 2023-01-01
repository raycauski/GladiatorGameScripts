using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for ranged gameplay, where character can  move and aim.
public class PlayerRangedState : PlayerBaseState
{
    private PlayerInputManager playerInput;
    private GameObject playerCameraHolder;

    private PlayerStateMachine stateMachine;

    private float maxSpeed = 4.5f;
    private float accelerationRate = 6.2f; // 10f
    private float maxSpeedAiming;
    private float aimSpeedMultiplier;


    
    public override void EnterState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        playerInput = playerStateMachine.playerInput;
        playerCameraHolder = playerStateMachine.playerCameraHolder;

        maxSpeedAiming = maxSpeed * aimSpeedMultiplier;

        playerStateMachine.SetCurrentMovement(maxSpeed, accelerationRate);
    }
    public override void LogicUpdate()
    {
        CheckStateChange();

       
        
        stateMachine.playerHands.SetBlockAnimation(playerInput.isBlocking);
        

    }
    public override void ExitState()
    {
    
    }

    private void OnEnable()
    {
        PlayerInputManager.dashEvent += Dash;
        PlayerInputManager.attackEvent += Attack;
        PlayerInputManager.parryEvent += Parry;

    }
    private void OnDisable()
    {
        PlayerInputManager.dashEvent -= Dash;
        PlayerInputManager.attackEvent -= Attack;
        PlayerInputManager.parryEvent -= Parry;
    }


    private void CheckStateChange()
    {
        if (playerInput.isCrouching)
        {
            stateMachine.ChangeState(stateMachine.playerCrouchState);
        }
        else if (playerInput.isSprinting)
        {
            if (playerInput.movementInput != Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.playerSprintState);
            }
        }
    }


    public void Dash()
    {
            stateMachine.ChangeState(stateMachine.playerDashState);
    }

    public void Attack()
    {
        stateMachine.playerHands.PlayAttackAnim();
    }

    public void Parry()
    {
        stateMachine.playerHands.PlayParryAnimation();

    }

  

}
