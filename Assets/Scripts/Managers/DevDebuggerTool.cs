using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevDebuggerTool : MonoBehaviour
{
    // Debugging tool used for viewing important variables and game states live in the game
    private GameBaseState currentGameState;
    private GameStateMachine gameStateMachine;
    private PlayerStateMachine playerStateMachine;
    private void Start()
    {
        gameStateMachine = GameStateMachine.Instance;
        if (!GameManager.Instance.Player)
        {
            return;
        }
        playerStateMachine = GameManager.Instance.Player.GetComponent<PlayerStateMachine>();
    }
    private void OnGUI()
    {
        if (playerStateMachine == null)
        {
            return;
        }
        // shows current game state in top left corner
        currentGameState = GameStateMachine.Instance.currentState;
        GUI.Label(new Rect(20, 20, 400, 30), "Game State:" + currentGameState.ToString());
        if (currentGameState == GameStateMachine.Instance.playerGameplayState && playerStateMachine != null)
        {
            // Shows current player state in top right corner if available
            GUI.Label(new Rect(680, 20, 400, 30), ("Player State:" + playerStateMachine.CurrentState).ToString());
            GUI.Label(new Rect(680, 50, 400, 30), ("Speed:" + playerStateMachine.PlayerMovement.playerController.velocity).ToString());
            //GUI.Label(new Rect(680, 80, 400, 30), ("DashVelocity " + playerStateMachine.playerMovement.dashVelocity).ToString());
        }
        
    }
}
