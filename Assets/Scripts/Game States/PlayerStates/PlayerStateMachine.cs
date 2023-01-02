using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The state machine which handles logic for player states. Manages each substate, but also applies logic universal
// to all player states such as gravity and movement.
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState currentState { get; private set; }
    public PlayerBaseState previousState { get; private set; }

    public PlayerInputManager playerInput { get; private set; }

    public PlayerSharedMovement playerMovement { get; private set; }
    /*
    public PlayerBaseState playerRangedState = new PlayerRangedState();
    public PlayerBaseState playerFallState = new PlayerFallState();
    public PlayerBaseState playerDashState = new PlayerDashState();
    public PlayerBaseState playerSprintState = new PlayerSprintState();
    public PlayerBaseState playerCrouchState = new PlayerCrouchState();
    */
    public PlayerBaseState playerRangedState;
    public PlayerBaseState playerFallState;
    public PlayerBaseState playerDashState;
    public PlayerBaseState playerSprintState;
    public PlayerBaseState playerCrouchState;

   

    private void Start()
    {

        playerMovement = gameObject.GetComponent<PlayerSharedMovement>();
        playerInput = GameStateMachine.Instance.GetComponent<PlayerInputManager>();

        playerRangedState = gameObject.AddComponent<PlayerRangedState>();
        playerFallState = gameObject.AddComponent<PlayerFallState>();
        playerDashState = gameObject.AddComponent<PlayerDashState>();
        playerSprintState = gameObject.AddComponent<PlayerSprintState>();
        playerCrouchState = gameObject.AddComponent<PlayerCrouchState>();

        // enters player Ranged State by default on startup.
        playerRangedState.EnterState();
        currentState = playerRangedState;
    }
    private void Update()
    {
        // Allows states to pass update logic to player state machine.
        currentState.LogicUpdate();
        playerMovement.LogicUpdate();
    }

    public void ChangeState(PlayerBaseState newState)
    {
        // Changes between states, storing previous and new states and applying enter/exit logic for current state.
        previousState = currentState;
        currentState.ExitState();
        currentState.enabled = false;
        newState.enabled = true;
        currentState = newState;
        
        newState.EnterState();
    }
}
