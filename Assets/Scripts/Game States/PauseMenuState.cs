using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuState : GameBaseState
{
    private GameObject pauseMenu;
    public override void EnterState(GameStateMachine gameStateMachine)
    {
        gameStateMachine.pauseInputManager.enabled = true;
        PauseGame();
    }

    public override void LogicUpdate(GameStateMachine gameStateMachine)
    {
        
    }
    public override void ExitState(GameStateMachine gameStateMachine)
    {
        UnpauseGame();
        gameStateMachine.pauseInputManager.enabled = false; 
    }

    private void PauseGame()
    {
        //pause time
        pauseMenu = GameManager.Instance.GetPauseMenu();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void UnpauseGame()
    {
        //unpause time
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        
    }
}
