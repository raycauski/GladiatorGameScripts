using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    private PlayerInputManager playerInput;
    [SerializeField] private GameObject player;
    private Transform playerTransform;
    private CharacterController playerController;
    [SerializeField] private Transform cameraTransform;
    private Transform holderTransform;
    [SerializeField] private Transform targetTransform;

    public float sensitivityX = 3f;
    public float sensitivityY = 2.5f;
    private const float SENSITIVITY = 20f; 
    private float maxPivot = 80f;
    private float minPivot = -80f;
    private float camAngleX = 0;
    private float camAngleY = 0;
    public bool horizontalAimIsInverted = false;
    public bool verticalAimIsInverted = false;
    private int aimXInvert = 1;
    private int aimYInvert = 1;

    public float rotationSpeed = 50f;

    [SerializeField] private bool headBobEnabled = true;

    [SerializeField, Range(0, 0.005f)] private float amplitude;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;
    private float toggleSpeed = 1.0f;
    
    private float returnToStartSpeed = 4f;
    private Vector3 startPos;
    private float sinTimer = 0.0f;
    private const float LERPSPEED = 1000f;
    private const float TARGETFOCUSDISTANCE = 15.0f;

    private Vector3 camRotation = Vector3.zero;
    private Vector3 headOffset = Vector3.zero;

    private void Awake()
    {
        // Record starting local position of camera to return to when not moving.
        startPos = cameraTransform.localPosition;
        playerTransform = player.transform;
        playerController = player.GetComponent<CharacterController>();
    }
    void Start()
    {
        headOffset = new Vector3(0f, 0.68f, 0f);
        playerInput = GameStateMachine.Instance.playerInputManager;
        holderTransform = this.transform;

        // Applies invert aim
        if (horizontalAimIsInverted)
        {
            aimXInvert = -1;
        }
        if (verticalAimIsInverted)
        {
            aimYInvert = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Copy playr transform and rotate camera per frame
        FollowPlayer();
        RotateCamera(Time.deltaTime, sensitivityX, sensitivityY);
        if (headBobEnabled)
        {
            // Record change in time to be used in Sin wave function
            sinTimer += Time.deltaTime;
            CheckMotion();
            ResetPosition(Time.deltaTime);
            ForwardTarget();
            // Camera faces the direction of the focus target to straighten camera direction
            cameraTransform.LookAt(targetTransform);
        }
        RotatePlayer();
    }

    private void FollowPlayer()
    {
        // Copies player transform with height offset to ensure non-jittery camera follow
        Vector3 targetPosition = playerTransform.position + headOffset;
        holderTransform.position = targetPosition;
    }

    private void RotateCamera(float delta, float sensX, float sensY)
    {
        // Uses player Look input to rotate the camera and player.
        Vector2 viewDirection = delta * SENSITIVITY * playerInput.lookInput;
        float viewX = sensX * viewDirection.x * aimXInvert;
        float viewY = sensY * viewDirection.y * aimYInvert;

        camAngleX += viewX;
        camAngleY -= viewY;

        // Clamps vertical angle between pivot angle min and max
        camAngleY = Mathf.Clamp(camAngleY, minPivot, maxPivot);

        camRotation.y = camAngleX;
        camRotation.x = camAngleY;
        
        Quaternion targetRotation = Quaternion.Euler(camRotation);
        holderTransform.localRotation = targetRotation;    
    }
    private void CheckMotion()
    {
        // Checks if the player passes toggleSpeed threshold and is grounded to apply head bobbing motion
        float speed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;

        if (speed > toggleSpeed && playerController.isGrounded)
        {
            PlayMotion(FootstepBobbing());
        }
    }
    
    private void ForwardTarget()
    {
        // Creates a target 15 meters in front of camera so that camera faces same direction while bobbing
        targetTransform.eulerAngles = new Vector3(holderTransform.eulerAngles.x, holderTransform.eulerAngles.y, 0.0f);
        Vector3 targetOffset = playerTransform.position + headOffset;
        targetOffset += holderTransform.forward * TARGETFOCUSDISTANCE;
        targetTransform.position = targetOffset;
    }
    private void PlayMotion(Vector3 motion)
    {
        // Applies local movement from parameter to camera. 
        Vector3 cameraTargetPos = (cameraTransform.localPosition + motion);
        // uses lerping so that deltaTime is applied, preventing unwanted movement.
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraTargetPos, LERPSPEED * Time.deltaTime);

    }
    private Vector3 FootstepBobbing()
    {
        // Generates sin wave motion to simulate camera bobbing, returns local pos vector for PlayMotion method.
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(sinTimer  * frequency) * amplitude;
        pos.x += Mathf.Cos(sinTimer * frequency / 2) * amplitude * 0.5f;
        return pos;
    }

    private void ResetPosition(float delta)
    {
        // Resets camera local position to starting position when no longer moving
        if (cameraTransform.localPosition != startPos)
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, startPos, returnToStartSpeed * delta);
        }
    }

    private void RotatePlayer()
    {
        // Rotates player controller to face same y angle as camera. 
        float targetRotation = Mathf.LerpAngle(playerTransform.transform.eulerAngles.y, holderTransform.transform.eulerAngles.y, rotationSpeed * Time.deltaTime);
        playerTransform.transform.eulerAngles = new Vector3(0, targetRotation, 0);
    }
    
}
