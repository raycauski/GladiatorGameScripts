using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSharedMovement : MonoBehaviour
{
    public PlayerStateMachine StateMachine;
    public CharacterController playerController;
    public PlayerInputManager PlayerInput { get; private set; }
    public PlayerHandsController playerHands;

    public GameObject playerCameraHolder;

    public Vector3 playerVelocity = Vector3.zero;

    public float gravity = 12f;
    private readonly float terminalVelocity = -60;

    private float velX = 0;
    private float velZ = 0;
    [SerializeField] private float maxSpeed = 4.5f;
    [SerializeField] private float accelerationRate = 10f;

    public bool isDashing = false;
    [SerializeField] private float dashDeceleration = 6f;
    public Vector3 dashVelocity { get; private set; } = Vector3.zero;

    private bool isJumping = false;
    public bool inCoyoteTime = false;
    private const float COYOTE_TIME = 0.15f;


    private void Start()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
        playerController = GetComponent<CharacterController>();
        PlayerInput = StateMachine.PlayerInput;
        playerHands = GetComponent<PlayerHandsController>();

    }
    public void LogicUpdate()
    {
        CheckFalling();
        ApplyGravity();
        HandleMovement();
        CheckStateChange();
    }

    private void HandleMovement()
    {
        // Input Vectors
        if (PlayerInput == null)
        {
            return;
        }
        Vector2 movementInput = (PlayerInput.movementInput).normalized;


        Vector2 targetVelocity = movementInput * maxSpeed;
        velX = Mathf.Lerp(velX, targetVelocity.x, Time.deltaTime * accelerationRate);
        velZ = Mathf.Lerp(velZ, targetVelocity.y, Time.deltaTime * accelerationRate);

        // calculates velocity using direction of player
        Vector3 currentVelocity = Quaternion.Euler(0, playerController.transform.eulerAngles.y, 0) * new Vector3(velX, playerVelocity.y, velZ);

        // Apply Standard Movement per frame
        if (isDashing)
        {
            if (playerVelocity.y < 0)
            {
                dashVelocity = Vector3.Lerp(dashVelocity, new Vector3(0, playerVelocity.y, 0), dashDeceleration * Time.deltaTime);
            }
            playerController.Move(dashVelocity * Time.deltaTime); // applies dash-specific velocity while dodging
        }
        else
        {
            playerController.Move(currentVelocity * Time.deltaTime); // standard movement for most cases
        }
    }
    private void ApplyGravity()
    {
        if (isDashing)
        {
            return;
        }
        // Grounded Check
        if (IsGrounded())
        {
            //playerVelocity.y = 25 * -gravity * Time.deltaTime;
            playerVelocity.y = -gravity * Time.deltaTime;
        }
        //Apply Gravity
        playerVelocity.y -= gravity * Time.deltaTime;
        playerVelocity.y = Mathf.Clamp(playerVelocity.y, terminalVelocity, 25f);
    }
    private void CheckFalling()
    {
        if (StateMachine.CurrentState == StateMachine.PlayerFallState)
        {
            return;
        }
        if (StateMachine.CurrentState == StateMachine.PlayerDashState)
        {
            isJumping = true;
            return;
        }
        if (!IsGrounded())
        {
            if (!isJumping)
            {
                StartCoroutine(CoyoteTimer());
                //isJumping = true;
            }

            if (!inCoyoteTime)
            {
                StateMachine.ChangeState(StateMachine.PlayerFallState);
            }
        }
    }
    public bool IsGrounded()
    {
        //Checks if character is grounded or stepping on a stair ledge to return grounded value
        if (playerController.isGrounded || IsOnStairStep())
        {
            isJumping = false;
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

    private void CheckStateChange()
    {

        PlayerBaseState currentState = StateMachine.CurrentState;
        if (currentState == StateMachine.PlayerFallState || currentState == StateMachine.PlayerRangedState)
        {
            return;
        }

        /*
        // Attack and parry override all states 
        if (PlayerInput.attack.triggered || PlayerInput.parry.triggered)
        {
            StateMachine.ChangeState(StateMachine.playerRangedState);
        }
        */
    }

    public void SetCurrentMovement(float newMaxSpeed, float newAcceleration)
    {
        // Called outside from other player states upon enterState to assign the movement parameters for that state.
        maxSpeed = newMaxSpeed;
        accelerationRate = newAcceleration;
    }

    private IEnumerator CoyoteTimer()
    {
        inCoyoteTime = true;
        yield return new WaitForSeconds(COYOTE_TIME);
        inCoyoteTime = false;
    }
    public IEnumerator PerformDash(Vector2 direction, float dashTime)
    {
        isDashing = true;
        dashVelocity = Quaternion.Euler(0, playerController.transform.eulerAngles.y, 0) *
            new Vector3(direction.x, playerVelocity.y, direction.y); // alligns direction with player facing direction
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
   
}
