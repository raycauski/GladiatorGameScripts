using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class GameBaseState
{
    [SerializeField] protected PlayerInputActions inputActions;
    //Abstract base state class inherited by all game states. Calls actions when entering and exiting states, and passes update logic
    // every frame through the state manager. Ref to state manager passed in every function
    public abstract void EnterState(GameStateMachine gameStateMachine);
    // Logic when entering current state
    //public abstract void LogicUpdate(GameStateMachine gameStateMachine);
    // Uses State machine monobehavior update to process update logic
  
    public abstract void ExitState(GameStateMachine gameStateMachine);
    // Logic when exiting state
}
