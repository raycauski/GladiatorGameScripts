using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The state machine which handles logic for player states. Manages each substate, but also applies logic universal
// to all player states such as gravity and movement.
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState currentState { get; private set; }
    public PlayerBaseState previousState { get; private set; }

    public PlayerBaseState playerRangedState = new PlayerRangedState();
    public PlayerBaseState playerFallState = new PlayerFallState();
    public PlayerBaseState playerDashState = new PlayerDashState();
    public PlayerBaseState playerSprintState = new PlayerSprintState();
    public PlayerBaseState playerCrouchState = new PlayerCrouchState();
    public CharacterController playerController { get; private set; }
    public PlayerInputManager playerInput { get; private set; }
    public GameObject playerCameraHolder;
    public Vector3 playerVelocity = Vector3.zero;
    
    public float gravity = 12f;
    private float terminalVelocity = -60;

    private float velX = 0;
    private float velZ = 0;
    private float maxSpeed = 4.5f;
    private float accelerationRate = 10f;

    private bool isDashing = false;
    public Vector3 dashVelocity { get; private set; } = Vector3.zero;


    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerInput = GameStateMachine.Instance.GetComponent<PlayerInputManager>();
        // enters player Ranged State by default on startup.
        playerRangedState.EnterState(this);
        currentState = playerRangedState;
    }
    private void Update()
    {
        // Allows states to pass update logic to player state machine.
        CheckFalling();
        ApplyGravity();
        HandleMovement();
        currentState.LogicUpdate(this);
        
    }

    private void OnEnable()
    {
        PlayerInputManager.dashEvent += Dash;
    }
    private void OnDisable()
    {
        PlayerInputManager.dashEvent -= Dash;
    }

    public void ChangeState(PlayerBaseState newState)
    {
        // Changes between states, storing previous and new states and applying enter/exit logic for current state.
        previousState = currentState;
        currentState.ExitState(this);
        currentState = newState;
        newState.EnterState(this);
    }

    private void HandleMovement()
    {
        // Input Vectors
        Vector2 movementInput = (playerInput.movementInput).normalized;
 

        Vector2 targetVelocity = movementInput * maxSpeed;
        velX = Mathf.Lerp(velX, targetVelocity.x, Time.deltaTime * accelerationRate);
        velZ = Mathf.Lerp(velZ, targetVelocity.y, Time.deltaTime * accelerationRate);

        // calculates velocity using direction of player
        Vector3 currentVelocity = Quaternion.Euler(0, playerController.transform.eulerAngles.y, 0) * new Vector3(velX, playerVelocity.y, velZ);

        // Apply Standard Movement per frame
        if (isDashing)
        {
            playerController.Move(dashVelocity * Time.deltaTime); // applies dash-specific velocity while dodging
        }
        else
        {
            playerController.Move(currentVelocity * Time.deltaTime); // standard movement for most cases
        }
        
        
       
    }
    private void ApplyGravity()
    {
        // Grounded Check
        if (IsGrounded())
        {
            playerVelocity.y = 25 * -gravity * Time.deltaTime;
        }
        //Apply Gravity
        playerVelocity.y -= gravity * Time.deltaTime;
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, terminalVelocity, 0);
    }
    private void CheckFalling()
    {
        if (currentState != playerFallState && currentState != playerDashState)
        {
            if (!IsGrounded())
            {
                ChangeState(playerFallState);
            }
        }
    }
    public bool IsGrounded()
    {
        //Checks if character is grounded or stepping on a stair ledge to return grounded value
        if (playerController.isGrounded || IsOnStairStep())
        {
            return true;
        }
        return false;
    }

    private bool IsOnStairStep()
    {
        // Checks if player has a stair step below them to maintain grounded status.
        float stepHeight = playerController.stepOffset;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, stepHeight))
        {
            //playerVelocity.y = -50; // EXPERIMENTAL: force step down velocity when over a step
            Debug.Log("Stairs");
            return true;
        }
        return false;
    }

    public void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            ChangeState(playerDashState);
            StartCoroutine(DashTimer());
        }
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(0.5f);
        isDashing = false;
        ChangeState(playerRangedState);
        dashVelocity = Vector3.zero;
    }

    public void SetDashVelocity(Vector3 newVelocity)
    {
        dashVelocity = newVelocity;
    }
 
    public void SetCurrentMovement(float newMaxSpeed, float newAcceleration)
    {
        // Called outside from other player states upon enterState to assign the movement parameters for that state.
        maxSpeed = newMaxSpeed;
        accelerationRate = newAcceleration;
    }



}
