using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuState : GameBaseState
{
    private GameObject player;

    public override void EnterState(GameStateMachine gameStateMachine)
    {
        // Enables input manager for current state 
        gameStateMachine.menuInputManager.enabled = true;
    }

    public override void LogicUpdate(GameStateMachine gameStateMachine)
    {

    }

    public override void ExitState(GameStateMachine gameStateMachine)
    {
        gameStateMachine.menuInputManager.enabled = false;

    }
}
