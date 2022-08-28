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
        playerStateMachine = GameManager.Instance.GetPlayer().GetComponent<PlayerStateMachine>();
    }
    private void OnGUI()
    {
        // shows current game state in top left corner
        currentGameState = GameStateMachine.Instance.currentState;
        GUI.Label(new Rect(20, 20, 400, 30), "Game State:" + currentGameState.ToString());
        if (currentGameState == GameStateMachine.Instance.playerGameplayState)
        {
            // Shows current player state in top right corner if available
            GUI.Label(new Rect(680, 20, 400, 30), ("Player State:" + playerStateMachine.currentState).ToString());
        }
        
    }
}
