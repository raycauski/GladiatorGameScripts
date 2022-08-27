using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private PlayerInput playerInput;

    // Actions
    private InputAction backAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputActions = new PlayerInputActions();
        // Create action mappings for all actions, then subscribe to functions
        CreateActions();
    }
    private void OnEnable()
    {
        inputActions.MenuControls.Enable();

    }
    private void OnDisable()
    {
        backAction.performed -= MenuBack;
        inputActions.MenuControls.Disable();
    }

    private void CreateActions()
    {
        // Creates action mappings for all actions, then subscribes to functions
        backAction = playerInput.actions["MenuBack"];
        backAction.performed += MenuBack;
    }

    // ======= Functions performed after key presses vvvvvvvv ==============

    private void MenuBack(InputAction.CallbackContext context)
    {
        Debug.Log("Going back...");
    }
}
