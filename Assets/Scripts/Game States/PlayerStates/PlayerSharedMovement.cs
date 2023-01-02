using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSharedMovement : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }
    public CharacterController playerController { get; private set; }
    public PlayerInputManager PlayerInput { get; private set; }
    public PlayerHandsController playerHands { get; private set; }

    public GameObject playerCameraHolder;

    public Vector3 playerVelocity = Vector3.zero;

    public float gravity = 12f;
    private float terminalVelocity = -60;

    private float velX = 0;
    private float velZ = 0;
    [SerializeField] private float maxSpeed = 4.5f;
    [SerializeField] private float accelerationRate = 10f;

    public bool isDashing = false;
    public Vector3 dashVelocity { get; private set; } = Vector3.zero;


    private void Start()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
        playerController = GetComponent<CharacterController>();
        PlayerInput = StateMachine.playerInput;
        playerHands = GetComponent<PlayerHandsController>();
    }
    public void LogicUpdate()
    {
        CheckFalling();
        ApplyGravity();
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Input Vectors
        Vector2 movementInput = (PlayerInput.movementInput).normalized;


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
        if (StateMachine.currentState != StateMachine.playerFallState && StateMachine.currentState != StateMachine.playerDashState)
        {
            if (!IsGrounded())
            {
                StateMachine.ChangeState(StateMachine.playerFallState);
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



    public void SetCurrentMovement(float newMaxSpeed, float newAcceleration)
    {
        // Called outside from other player states upon enterState to assign the movement parameters for that state.
        maxSpeed = newMaxSpeed;
        accelerationRate = newAcceleration;
    }
    
   
    public IEnumerator PerformDash()
    {
        yield break;
    }
   
}
