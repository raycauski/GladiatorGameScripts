using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float recoilSnappiness = 40f;
    private Vector3 targetRotation;
    private Vector3 currentRotation;
    private Camera playerCamera;
    private float FOV;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponent<Camera>();
        FOV = playerCamera.fieldOfView; 
    }

    // Update is called once per frame
    void Update()
    {
        LerpToTarget();
    }

    private void LerpToTarget()
    {
        targetRotation = Vector3.Lerp(targetRotation, new Vector3(targetRotation.x, targetRotation.y, 0f), recoilSnappiness * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, recoilSnappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
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
}
