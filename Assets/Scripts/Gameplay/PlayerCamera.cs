using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Collisions
    private Camera mainCamera;
    public LayerMask collisionLayers;
    public float minimumOffset = 0.5f;
    public float clippingBounds;
    private Vector3[] clippingPoints = new Vector3[4];
    private float clippingPlane;
    private float fieldOfView;
    private float aspect;
    private float forwardOffset;
    private bool isColliding;
    // Following
    public GameObject player;
    public Transform camPivot;
    public Transform camOrigin;
    public float followTime = 10f;
    public float rotationSpeed = 10f;
    private Transform playerPos;
    private Vector3 camFollowVelocity = Vector3.zero;
    private Vector3 followVelocity = Vector3.zero;
    // Rotations
    public float sensitivityX = 0.1f;
    public float sensitivityY = 0.1f;
    private float camAngleX;
    private float camAngleY;
    private float minPivot = -35f;
    private float maxPivot = 60f;
    // Aiming
    public CanvasGroup crosshairCanvas;
    public float aimSensitivity;
    public float aimFOV;
    public float zoomSpeed = 0.5f;
    private float defaultFOV;
    private float sensitivityFactor;
    private PlayerNavigation playerScript;

    private void Start()
    {
        // References current camera settings
        mainCamera = Camera.main;
        fieldOfView = mainCamera.fieldOfView;
        defaultFOV = mainCamera.fieldOfView;
        aimFOV = defaultFOV * 0.6f;
        clippingPlane = mainCamera.nearClipPlane;
        aspect = mainCamera.aspect;
        crosshairCanvas.alpha = 0;

        playerPos = player.transform;
        playerScript = player.GetComponent<PlayerNavigation>();
    }
    void LateUpdate()
    {
        // calculates collision corners, finds collisions, then applies rotation
        OriginFollowPlayer(Time.deltaTime);
        CameraPivotRotation(Time.deltaTime, sensitivityX, sensitivityY);

        UpdateCameraCollisionPoints();
        CameraCollisions(Time.deltaTime);
        CameraFollowOrigin(Time.deltaTime);

        AimZoom(Time.deltaTime);
    }
    public void OriginFollowPlayer(float delta)
    {
        // Camera Origin Empty follows player with smooth damp
        Vector3 targetPosition = Vector3.SmoothDamp(camOrigin.position, playerPos.position, ref camFollowVelocity, delta * followTime);
        camOrigin.position = targetPosition;
    }
    private void CameraPivotRotation(float delta, float sensX, float sensY)
    {
        
        float h = sensX * sensitivityFactor * Input.GetAxisRaw("Mouse X"); /// -1 inverts horizontal cam, add option later
        float v = sensY * sensitivityFactor * Input.GetAxisRaw("Mouse Y"); /// -1 inverts horizontal cam, add option later

        // Angle in radians
        camAngleX += h;
        camAngleY -= v;

        camAngleY = Mathf.Clamp(camAngleY, minPivot, maxPivot);

        Vector3 camRotation = Vector3.zero;
        camRotation.y = camAngleX;
        camRotation.x = camAngleY;
        Quaternion targetRotation = Quaternion.Euler(camRotation);
        camOrigin.localRotation = targetRotation;
    }
    public void CameraFollowOrigin(float delta)
    {
        // Camera smoothly copies camera pivot x and y rotations and position. Moves closer to player if colliding to avoid walls
        if (isColliding)
        {
            Vector3 innerDirection = (playerPos.position - camPivot.position).normalized;
            Vector3 forwardOffsetTarget = camPivot.position + (innerDirection * forwardOffset);
            Vector3 targetPosition = Vector3.SmoothDamp(transform.localPosition, forwardOffsetTarget, ref followVelocity, delta * followTime);
            transform.position = targetPosition;
        }
        else
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.localPosition, camPivot.position, ref followVelocity, delta * followTime);
            transform.position = targetPosition;
        }
        Vector3 targetRotation = new Vector3(
            Mathf.LerpAngle(transform.eulerAngles.x, camPivot.eulerAngles.x, delta * rotationSpeed),
            Mathf.LerpAngle(transform.eulerAngles.y, camPivot.eulerAngles.y, delta * rotationSpeed),
            0);
        transform.eulerAngles = targetRotation;
    }
  
    private void CameraCollisions(float delta)
    {
        // Raycasts check if anything is obstructing path between player and camera clipping plane
        isColliding = false;

        float[] minDistanceArray = new float[4];
        minDistanceArray[0] = 10f;
        minDistanceArray[1] = 10f;
        minDistanceArray[2] = 10f;
        minDistanceArray[3] = 10f;

        float distance = Vector3.Distance(camPivot.position, playerPos.position);

        for (int i = 0; i < clippingPoints.Length; i++)
        {
            Debug.DrawLine(playerPos.position, (camPivot.position + clippingPoints[i]), Color.red);

            RaycastHit hit;
            if (Physics.Linecast(playerPos.position, camPivot.position + clippingPoints[i], out hit, collisionLayers))
            {
                Debug.DrawLine(playerPos.position, (camPivot.position + clippingPoints[i]), Color.green);
                minDistanceArray[i] = hit.distance;
                isColliding = true;
            }
        }
        float minDistance = Mathf.Min(minDistanceArray);
        forwardOffset = distance - minDistance + minimumOffset;
    }
    public void UpdateCameraCollisionPoints()
    {
        // Updates raycast collision targets based on camera clipping plane and location
        clippingPoints = new Vector3[4]; // resets array per frame
        float horizontalOffset = Mathf.Abs(clippingPlane * Mathf.Tan(fieldOfView / clippingBounds)); // x
        float verticalOffset = horizontalOffset / aspect;                                           // y
        float forwardOffset = clippingPlane;                                                       // z

        // Assigns 4 corners of clipping plane to array
        clippingPoints[0] = (transform.localRotation * new Vector3(-horizontalOffset, verticalOffset, forwardOffset));
        clippingPoints[1] = (transform.localRotation * new Vector3(horizontalOffset, verticalOffset, forwardOffset));
        clippingPoints[2] = (transform.localRotation * new Vector3(-horizontalOffset, -verticalOffset, forwardOffset));
        clippingPoints[3] = (transform.localRotation * new Vector3(horizontalOffset, -verticalOffset, forwardOffset));
    }

    private void AimZoom(float delta)
    {
        //Zooming in decreases cam FOV and slows camera sensitivity by the aim sens factor
        if (Input.GetMouseButton(1))
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, aimFOV, delta * zoomSpeed);//FOV zoom in
            sensitivityFactor = Mathf.Lerp(sensitivityFactor, aimSensitivity, zoomSpeed); // Sensitivity slows
            crosshairCanvas.alpha = Mathf.Lerp(crosshairCanvas.alpha, 1, delta * zoomSpeed); // crosshair fades in
            playerScript.isAiming = true; // Player will rotate to follow camera as in PlayerMove Script
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFOV, delta * zoomSpeed); //FOV zooms out 
            sensitivityFactor = Mathf.Lerp(sensitivityFactor, 1, zoomSpeed); // sensitivity returns to normal
            crosshairCanvas.alpha = Mathf.Lerp(crosshairCanvas.alpha, 0, delta * zoomSpeed * 2); //crosshair fade out
            playerScript.isAiming = false; // Player will no longer rotate
        }
    }

}
