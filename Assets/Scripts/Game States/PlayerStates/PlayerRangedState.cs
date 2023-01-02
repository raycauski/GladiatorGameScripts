using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for ranged gameplay, where character can  move and aim.
public class PlayerRangedState : PlayerBaseState
{
    private float maxSpeed = 4.5f;
    private float accelerationRate = 6.2f; // 10f
    private float maxSpeedAiming;
    private float aimSpeedMultiplier;


    
    public override void EnterState()
    {
        maxSpeedAiming = maxSpeed * aimSpeedMultiplier;
        PlayerMovement.SetCurrentMovement(maxSpeed, accelerationRate);
    }
    public override void LogicUpdate()
    {
        CheckStateChange();
        PlayerMovement.playerHands.SetBlockAnimation(PlayerInput.isBlocking);

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
        if (PlayerInput.isCrouching)
        {
            StateMachine.ChangeState(StateMachine.playerCrouchState);
        }
        else if (PlayerInput.isSprinting)
        {
            if (PlayerInput.movementInput != Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.playerSprintState);
            }
        }
    }

    public void Dash()
    {
            StateMachine.ChangeState(StateMachine.playerDashState);
    }

    public void Attack()
    {
        // TODO: Change to wepon
        PlayerMovement.playerHands.PlayAttackAnim();
    }

    public void Parry()
    {
        // TODO change to weapon
        PlayerMovement.playerHands.PlayParryAnimation();

    }

  

}
