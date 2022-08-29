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
    private InputAction sprint;
    private InputAction crouch;
    private InputAction dash;

    public Vector2 movementInput { get; private set; } = Vector2.zero;
    public Vector2 lookInput { get; private set; } = Vector2.zero;
    public bool isAiming { get; private set; } = false;
    
    public bool isCrouching = false;
    public bool isSprinting = false;

    public delegate void OnDashPress();
    public static OnDashPress dashing;


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        CreateActions();

        aim.performed += context => SetAim(true);
        aim.canceled += context => SetAim(false);
    }
    private void CreateActions()
    {
        movement = inputActions.PlayerControls.Movement;
        aim = inputActions.PlayerControls.AimIn;
        look = inputActions.PlayerControls.Look;
        pause = inputActions.PlayerControls.Pause;
        sprint = inputActions.PlayerControls.Sprint;
        crouch = inputActions.PlayerControls.Crouch;
        dash = inputActions.PlayerControls.Dodge;
    }
    private void OnEnable()
    {

        inputActions.PlayerControls.Enable(); // toggles ranged controls

        movement.performed += SetMovement;
        movement.canceled += SetMovement;
        movement.canceled += ResetSprint;

        look.performed += SetLook;
        look.canceled += SetLook;

        sprint.performed += ToggleSprint;
        sprint.performed += ResetCrouch;

        crouch.performed += ToggleCrouch;
        crouch.performed += ResetSprint;

        dash.performed += CallDash;
        dash.performed += ResetCrouch;
        dash.performed += ResetSprint;

        pause.performed += PauseGame;
        pause.canceled += PauseGame;


    }
    private void OnDisable()
    {
        movement.performed -= SetMovement;
        movement.canceled -= SetMovement;
        movement.canceled -= ResetSprint;

        look.performed -= SetLook;
        look.canceled -= SetLook;

        sprint.performed -= ToggleSprint;
        sprint.performed -= ResetCrouch;
        
        crouch.performed -= ToggleCrouch;
        crouch.performed -= ResetSprint;

        dash.performed -= CallDash;
        dash.performed -= ResetCrouch;
        dash.performed -= ResetSprint;

        pause.performed -= PauseGame;
        pause.canceled -= PauseGame;

        ResetInputValues();
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
    private void SetAim(bool aimToggle)
    {
        isAiming = aimToggle;
    }
    private void ToggleCrouch(InputAction.CallbackContext context)
    {
        // Toggles between crouching
        isCrouching = !isCrouching;
    }
    private void ResetCrouch(InputAction.CallbackContext context)
    {
        isCrouching = false;
    }

    private void ToggleSprint(InputAction.CallbackContext context)
    {
        isSprinting = !isSprinting;
    }
    private void ResetSprint(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }
    /*
    private void SetSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed;
    }
    */
    private void CallDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dashing");
        dashing();
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.pauseMenuState);
    }

    private void ResetInputValues()
    {
        // Resets recorded input values to 0 when disabling controls to prevent unwanted movement
        movementInput = Vector2.zero; 
        lookInput = Vector2.zero;
        isSprinting = false;
    }

 
}
