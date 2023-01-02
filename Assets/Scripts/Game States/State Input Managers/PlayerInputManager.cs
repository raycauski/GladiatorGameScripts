using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{

    // Input
    private PlayerInputActions inputActions;

    // Actions
    // Movement
    public InputAction movement;
    public InputAction look;
    public InputAction dash;
    public InputAction sprint;
    public InputAction crouch;

    // Melee
    public InputAction attack;
    public InputAction attackHeavy;
    public InputAction attackSpecial;
    public InputAction parry;
    public InputAction block;

    // Gun
    public InputAction swap;
    public InputAction reload;
    public InputAction aim;
    public InputAction fire;

    // Menu
    public InputAction interact;
    public InputAction pause;
    public InputAction inventory;

    public Vector2 movementInput { get; private set; } = Vector2.zero;
    public Vector2 lookInput { get; private set; } = Vector2.zero;
    
    public bool isCrouching = false;
    public bool isSprinting = false;
    public bool isBlocking = false;

    private void Awake()
    {
        // TEST
        inputActions = new PlayerInputActions();
        CreateActions();
    }
    private void CreateActions()
    {
        movement =      inputActions.PlayerControls.Movement;
        look =          inputActions.PlayerControls.Look;
        dash =          inputActions.PlayerControls.Dodge;
        sprint =        inputActions.PlayerControls.Sprint;
        crouch =        inputActions.PlayerControls.Crouch;
        attack =        inputActions.PlayerControls.Attack;
        attackHeavy =   inputActions.PlayerControls.AttackHeavy;
        attackSpecial = inputActions.PlayerControls.AttackSpecial;
        parry =         inputActions.PlayerControls.Parry;
        block =         inputActions.PlayerControls.Block;
        swap =          inputActions.PlayerControls.Swap;
        reload =        inputActions.PlayerControls.Reload;
        aim =           inputActions.PlayerControls.AimIn;
        fire =          inputActions.PlayerControls.Fire;
        interact =      inputActions.PlayerControls.Interact;
        pause =         inputActions.PlayerControls.Pause;
        inventory =     inputActions.PlayerControls.Inventory;
    }
    private void OnEnable()
    {

        inputActions.PlayerControls.Enable(); // toggles ranged controls

        movement.performed += SetMovement;
        movement.canceled += SetMovement;

        look.performed += SetLook;
        look.canceled += SetLook;

        dash.performed += SetDash;
        dash.performed += ResetCrouch;
        dash.performed += ResetSprint;

        sprint.performed += ToggleSprint;
        sprint.canceled += ResetSprint;
        sprint.performed += ResetCrouch;

        crouch.performed += ToggleCrouch;
        crouch.performed += ResetSprint;

        attack.performed += SetAttack;

        attackHeavy.performed += SetAttackHeavy;

        attackSpecial.performed += SetAttackSpecial;

        parry.performed += SetParry;

        block.performed += SetBlock;
        block.canceled += ResetBlock;

        swap.performed += SetSwap;

        reload.performed += SetReload;

        aim.performed += SetAim;

        fire.performed += SetFire;

        interact.performed += SetInteract;

        pause.performed += PauseGame;

    }
    private void OnDisable()
    {
        movement.performed -= SetMovement;
        movement.canceled -= SetMovement;

        look.performed -= SetLook;
        look.canceled -= SetLook;

        dash.performed -= SetDash;
        dash.performed -= ResetCrouch;
        dash.performed -= ResetSprint;

        sprint.performed -= ToggleSprint;
        sprint.canceled -= ResetSprint;
        sprint.performed -= ResetCrouch;

        crouch.performed -= ToggleCrouch;
        crouch.performed -= ResetSprint;

        attack.performed -= SetAttack;

        attackHeavy.performed -= SetAttackHeavy;

        attackSpecial.performed -= SetAttackSpecial;

        parry.performed -= SetParry;

        block.performed -= SetBlock;
        block.canceled -= ResetBlock;

        swap.performed -= SetSwap;

        reload.performed -= SetReload;

        aim.performed -= SetAim;

        fire.performed -= SetFire;

        interact.performed -= SetInteract;

        pause.performed -= PauseGame;

        ResetInputValues();
        inputActions.PlayerControls.Disable();
    }

    // MOVEMENT  ---------------------------------------------------------------
    private void SetMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    // LOOK      ---------------------------------------------------------------
    private void SetLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    // DODGE     ---------------------------------------------------------------
    private void SetDash(InputAction.CallbackContext context)
    {
        //Debug.Log("Dashing");
        /*
        if (dashEvent != null)
        {
            dashEvent();
        }
        */
    }

    // SPRINT    ---------------------------------------------------------------
    private void ToggleSprint(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }
    private void ResetSprint(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    // CROUCH    ---------------------------------------------------------------
    private void ToggleCrouch(InputAction.CallbackContext context)
    {
        // Toggles between crouching
        isCrouching = !isCrouching;
    }
    private void ResetCrouch(InputAction.CallbackContext context)
    {
        isCrouching = false;
    }

    // ATTACK    ---------------------------------------------------------------
    private void SetAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("Attack");
        /*
        if (attackEvent != null)
        {
            attackEvent();
        }
        */
    }

    // HEAVY     ---------------------------------------------------------------
    private void SetAttackHeavy(InputAction.CallbackContext context)
    {
        //Debug.Log("Heavy Attack");
        /*
        if (attackHeavyEvent != null)
        {
            attackHeavyEvent();
        }
        */
    }

    // SPECIAL   ---------------------------------------------------------------
    private void SetAttackSpecial(InputAction.CallbackContext context)
    {
        // Debug.Log("Special Attack");
        /*
        if (attackSpecialEvent != null)
        {
            attackSpecialEvent();
        }
        */
    }

    // PARRY     ---------------------------------------------------------------
    private void SetParry(InputAction.CallbackContext context)
    {
        //Debug.Log("Parry");
        /*
        if (parryEvent != null)
        {
            parryEvent();
        }
        */
    }

    public void ResetStates()
    {
        isBlocking = false;
        isSprinting = false;
        isCrouching = false;
    }
    // BLOCK     ---------------------------------------------------------------
    private void SetBlock(InputAction.CallbackContext context)
    {
        isBlocking = true;
        //Debug.Log("Block");
    }

    private void ResetBlock(InputAction.CallbackContext context)
    {
        isBlocking = false;
        //Debug.Log("Block Cancelled");
    }
    // SWAP      ---------------------------------------------------------------
    private void SetSwap(InputAction.CallbackContext context)
    {
        //Debug.Log("Swapping");
    }
    // RELOAD    ---------------------------------------------------------------
    private void SetReload(InputAction.CallbackContext context)
    {
        //Debug.Log("Reloading");
    }
    // AIM       ---------------------------------------------------------------
    private void SetAim(InputAction.CallbackContext context)
    {
        //Debug.Log("Aiming");
    }
    // FIRE      ---------------------------------------------------------------
    private void SetFire(InputAction.CallbackContext context)
    {
       // Debug.Log("Firing");
    }
    // INTERACT  ---------------------------------------------------------------
    private void SetInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("Interacting");
    }
    // PAUSE     ---------------------------------------------------------------
    private void PauseGame(InputAction.CallbackContext context)
    {
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.pauseMenuState);
    }
    // INVENTORY ---------------------------------------------------------------


    private void ResetInputValues()
    {
        // Resets recorded input values to 0 when disabling controls to prevent unwanted movement
        movementInput = Vector2.zero; 
        lookInput = Vector2.zero;
        isSprinting = false;
    }

 
}
