using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayState : GameBaseState
{
    private GameObject player;
    private PlayerStateMachine playerStateMachine;
    public override void EnterState(GameStateMachine gameStateMachine)
    {
        player = GameManager.Instance.GetPlayer();
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerStateMachine.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameStateMachine.playerInputManager.enabled = true;
    }

    public override void LogicUpdate(GameStateMachine gameStateMachine)
    {
        
    }

    public override void ExitState(GameStateMachine gameStateMachine)
    {
        playerStateMachine.enabled = false;
        gameStateMachine.playerInputManager.enabled = false;
    }
}
