using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    // Input
    private PlayerInputActions inputActions;

    // Actions
    private InputAction movement;
    private InputAction aim;
    private InputAction look;
    private InputAction pause;

    public Vector2 movementInput { get; private set; } = Vector2.zero;
    public Vector2 lookInput { get; private set; } = Vector2.zero;
    public bool isAiming { get; private set; } = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        CreateActions();

        aim.performed += context => SetAimTrue();
        aim.canceled += context => SetAimFalse();
    }
    private void CreateActions()
    {
        movement = inputActions.PlayerControls.Movement;
        aim = inputActions.PlayerControls.AimIn;
        look = inputActions.PlayerControls.Look;
        pause = inputActions.PlayerControls.Pause;
    }
    private void OnEnable()
    {

        inputActions.PlayerControls.Enable(); // toggles ranged controls

        movement.performed += SetMovement;
        movement.canceled += SetMovement;

        look.performed += SetLook;
        look.canceled += SetLook;

        pause.performed += PauseGame;
        pause.canceled += PauseGame;


    }
    private void OnDisable()
    {
        movement.performed -= SetMovement;
        movement.canceled -= SetMovement;

        look.performed -= SetLook;
        look.canceled -= SetLook;

        pause.performed -= PauseGame;
        pause.canceled -= PauseGame;

        inputActions.PlayerControls.Disable();
    }

    private void SetMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    private void SetLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    private void SetAimTrue()
    {
        isAiming = true;
        Debug.Log("Woaoooaooaoaoa");
    }
    private void SetAimFalse()
    {
        isAiming = false;
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.pauseMenuState);
    }

 
}