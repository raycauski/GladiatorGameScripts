using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The state machine which handles logic for player states. Manages each substate, but also applies logic universal
// to all player states such as gravity and movement.
public class PlayerStateMachine : MonoBehaviour
{
    /// References
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerBaseState PreviousState { get; private set; }

    public PlayerInputManager PlayerInput { get; private set; }

    public PlayerSharedMovement PlayerMovement { get; private set; }

    /// States
    public PlayerBaseState PlayerRangedState { get; private set; }
    public PlayerBaseState PlayerFallState { get; private set; }
    public PlayerBaseState PlayerDashState { get; private set; }
    public PlayerBaseState PlayerSprintState { get; private set; }
    public PlayerBaseState PlayerCrouchState { get; private set; }

    private void Start()
    {
        PlayerMovement = gameObject.GetComponent<PlayerSharedMovement>();
        PlayerInput = GameStateMachine.Instance.GetComponent<PlayerInputManager>();

        PlayerRangedState = gameObject.AddComponent<PlayerRangedState>();
        PlayerFallState = gameObject.AddComponent<PlayerFallState>();
        PlayerDashState = gameObject.AddComponent<PlayerDashState>();
        PlayerSprintState = gameObject.AddComponent<PlayerSprintState>();
        PlayerCrouchState = gameObject.AddComponent<PlayerCrouchState>();

        // enters player Ranged State by default on startup.
        CurrentState = PlayerRangedState;
        PlayerRangedState.EnterState();
        
    }
    private void Update()
    {
        // Allows states to pass update logic to player state machine.
        CurrentState.LogicUpdate();
        PlayerMovement.LogicUpdate();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        // Changes between states, storing previous and new states and applying enter/exit logic for current state.
        PreviousState = CurrentState;
        CurrentState.ExitState();
        CurrentState.enabled = false;
        newState.enabled = true;
        CurrentState = newState;
        
        newState.EnterState();
    }
}
