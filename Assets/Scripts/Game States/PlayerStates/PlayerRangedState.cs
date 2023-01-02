using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
   

        if (PlayerInput.attack.triggered)
        {
            Attack();
        }
        //PlayerMovement.playerHands.SetBlockAnimation(PlayerInput.isBlocking);

    }
    public override void ExitState()
    {
    
    }

    private void OnEnable()
    {
        PlayerInput.dash.performed += Dash;
       // PlayerInput.attack.performed += Attack;
        PlayerInput.parry.performed += Parry;

    }
    private void OnDisable()
    {
        PlayerInput.dash.performed -= Dash;
        //PlayerInput.attack.performed -= Attack;
        PlayerInput.parry.performed -= Parry;
    }


    private void CheckStateChange()
    {
        if (PlayerInput.isCrouching)
        {
            StateMachine.ChangeState(StateMachine.PlayerCrouchState);
        }
        else if (PlayerInput.sprint.IsPressed())
        {
            if (PlayerInput.movementInput != Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.PlayerSprintState);
            }
        }
    }

    public void Dash(InputAction.CallbackContext context) 
    {
            StateMachine.ChangeState(StateMachine.PlayerDashState);
    }

    public void Attack()
    {
        // TODO: Change to wepon
        PlayerMovement.playerHands.PlayAttackAnim();
    }

    public void Parry(InputAction.CallbackContext context)
    {
        // TODO change to weapon
        PlayerMovement.playerHands.PlayParryAnimation();

    }

  

}
