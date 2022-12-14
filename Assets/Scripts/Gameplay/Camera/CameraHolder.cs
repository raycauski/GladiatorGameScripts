using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    private PlayerInputManager playerInput;
    [SerializeField] private GameObject player;
    private Transform playerTransform;
    private CharacterController playerController;
    [SerializeField] private Camera playerCamera;
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
    [SerializeField] private bool headBobEnabled = false;

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
    private float standingHeight = 0.68f;
    private float crouchHeight = 0.16f;
    private bool isCrouching = false;
    

    public float recoilSnappiness = 2f;
    private Vector3 targetRotation;
    private Vector3 currentRotation;

    private void Awake()
    {
        InitializeVars();
        
    }
    void Start()
    {
        playerInput = GameStateMachine.Instance.PlayerInputManager;
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
        LerpToTarget();


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

        CameraCrouch();

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

        //camAngleX += viewX;
        //camAngleY -= viewY;

        // Clamps vertical angle between pivot angle min and max
        //camAngleY = Mathf.Clamp(camAngleY, minPivot, maxPivot);

        //camRotation.y = camAngleX;
        //camRotation.x = camAngleY;

        Vector3 camRotation = new Vector3(-viewY, viewX, 0f);
        currentRotation += camRotation;
        targetRotation += camRotation;


    }
    private void LerpToTarget()
    {
        targetRotation = new Vector3(Mathf.Clamp(targetRotation.x, minPivot, maxPivot), targetRotation.y, targetRotation.z);
        currentRotation = new Vector3(Mathf.Clamp(currentRotation.x, minPivot, maxPivot), currentRotation.y, currentRotation.z);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, recoilSnappiness * Time.deltaTime);
        holderTransform.localRotation = Quaternion.Euler(currentRotation);
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

    private void CameraCrouch()
    {
        if (isCrouching)
        {
            Vector3 currentTargetHeight = new Vector3(0, crouchHeight, 0);
            headOffset = Vector3.Lerp(headOffset, currentTargetHeight, returnToStartSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 currentTargetHeight = new Vector3(0, standingHeight, 0);
            headOffset = Vector3.Lerp(headOffset, currentTargetHeight, returnToStartSpeed * Time.deltaTime);
        }
    }

    public void SetCrouching(bool crouchToggle)
    {
        isCrouching = crouchToggle;
    }

    private void RotatePlayer()
    {
        // Rotates player controller to face same y angle as camera. 
        float targetRotation = Mathf.LerpAngle(playerTransform.transform.eulerAngles.y, holderTransform.transform.eulerAngles.y, rotationSpeed * Time.deltaTime);
        playerTransform.transform.eulerAngles = new Vector3(0, targetRotation, 0);
    }

    public void ApplyRecoil(float spreadX, float recoil)
    {
        targetRotation += new Vector3(-recoil, -spreadX * recoil * 20f, 0f);
    }
    public void WarpFOV(float degrees, float aimSpeed)
    {
        float currentFOV = playerCamera.fieldOfView;
        float targetFOV = Mathf.Lerp(currentFOV, degrees, aimSpeed * Time.deltaTime);
        playerCamera.fieldOfView = targetFOV;
    }

    private void InitializeVars()
    {
        // Record starting local position of camera to return to when not moving.
        startPos = cameraTransform.localPosition;
        playerTransform = player.transform;
        playerController = player.GetComponent<CharacterController>();
        headOffset.y = standingHeight;

    }
    
}
