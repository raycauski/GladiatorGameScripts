using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInputManager : MonoBehaviour
{
    // Input
    private PlayerInputActions inputActions;
 
    private InputAction menuBack;
    private InputAction menuSelect;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        CreateActions();
    }
    private void CreateActions()
    {
        menuBack = inputActions.MenuControls.MenuBack;
        menuSelect = inputActions.MenuControls.Select;
    }
    private void OnEnable()
    {
        inputActions.MenuControls.Enable();

        menuBack.performed += GoBack;
        menuBack.canceled += GoBack;
    }
    private void OnDisable()
    {
        menuBack.performed -= GoBack;
        menuBack.canceled -= GoBack;

        inputActions.MenuControls.Disable();
    }
    private void GoBack(InputAction.CallbackContext context)
    {
        // go back, but if at home screen then unpause.
        UnPauseGame();
    }
    private void UnPauseGame()
    {
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.previousState);
    }
}
