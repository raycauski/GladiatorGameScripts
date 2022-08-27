using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The state machine which handles logic for player states. Manages 
public class PlayerStateMachine : MonoBehaviour
{
    PlayerBaseState currentState;
    PlayerBaseState previousState;

    public PlayerBaseState playerRanged = new PlayerRangedState();
    public CharacterController playerController { get; private set; }
    public PlayerInputManager playerInput { get; private set; }
    public GameObject playerCameraHolder;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerInput = GameStateMachine.Instance.GetComponent<PlayerInputManager>();
        // enters player Ranged State by default on startup.
        playerRanged.EnterState(this);
        currentState = playerRanged;
    }
    private void Update()
    {
        // Allows states to pass update logic to player state machine.
        currentState.LogicUpdate(this);
    }

    public void ChangeState(PlayerBaseState newState)
    {
        // Changes between states, storing previous and new states and applying enter/exit logic for current state.
        previousState = currentState;
        currentState.ExitState(this);
        currentState = newState;
        newState.EnterState(this);
    }

}
