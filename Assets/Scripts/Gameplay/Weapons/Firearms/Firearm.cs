using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Firearm : MonoBehaviour
{
    /// References
    private Camera playerCamera;
    private CharacterController playerController;
    private float playerMaxSpeed = 8f; // Set as max sprint speed, change later to refernce
    public LayerMask playerLayerMask;

    private Ray raycast;
    private RaycastHit raycastHit;
    private LineRenderer lineRenderer;


    /// variables
    ///<value> Max number of rounds per magazine</value>
    [SerializeField] 
    protected int capacity;
    ///<value> Current number of rounds in magazine </value>
    protected int currentMag;
    ///<value> Total ammo reserve rounds </value>
    [SerializeField]
    protected int maxAmmo;
    ///<value> Current amount of reserve ammo </value>
    protected int currentAmmo;
    ///<value> Strength of vertical recoil in lb-fps</value>
    [SerializeField]
    protected float verticalRecoil;
    ///<value> Strength of horizontal recoil in lb-fps </value>
    [SerializeField]
    protected float horizontalRecoil;
    ///<value> Fire rate in rounds per minute </value>
    [SerializeField]
    protected float fireRateRPM;
    ///<value> Damage points to apply to enemy per bullet </value>
    [SerializeField]
    protected int damage;
    ///<value> How many seconds it takes to fully reload the weapon </value>
    [SerializeField]
    protected float reloadTimeSeconds;
    ///<value> Weight of weapon in lbs determining how slow its handling will be </value>
    [SerializeField]
    protected float weaponWeight;
    public bool isAutomatic = false;

    protected float aimTime;
    /// constants
    protected float fireDelay;


    private bool isWaiting = false;

    protected const float AIM_DELAY_MULTIPLIER = 2f / 15f;
    protected const float MAX_RANGE = 250f;
    protected const float MIN_DEFAULT_SPREAD = 0.2f;

    private void OnDisable()
    {
        CancelInvoke(nameof(WaitForReload));
    }


    // Start is called before the first frame update
    void Start()
    {
        fireDelay = CalculateFireDelaySeconds(fireRateRPM);
        //Debug.Log("Fire delay = " + fireDelay + " seconds");
        aimTime = CalculateAimDelaySeconds(weaponWeight);
        //Debug.Log("Aim delay seconds = " + aimTime + " seconds, using " + AIM_DELAY_MULTIPLIER + " seconds per pound");
        GameObject player = GameManager.Instance.Player;
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        playerController = player.GetComponent<PlayerSharedMovement>().playerController;
        //playerMaxSpeed = player.GetComponent<PlayerSharedMovement>().maxSpeed;

        // Testing
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        
        currentMag = capacity;
        currentAmmo = maxAmmo;
    }

    public virtual void Fire()
    {
        if (!CanShoot())
        {
            return;
        }
        StartCoroutine(ShootRound());
    }


    public void AimIn()
    {
        //Debug.Log("AIMING IN");
    }

    public void AimOut()
    {
        //Debug.Log("AIMED OUT");
    }

    private IEnumerator ShootRound()
    {
        Debug.Log("SHOOTING");
        isWaiting = true;
        Vector3 bulletDirection = CalculateBulletDirection();
        /// RecoilScript.ApplyRecoil(bulletDirection);
        if (Physics.Raycast(playerCamera.transform.position, bulletDirection, out raycastHit, MAX_RANGE, playerLayerMask))
        {
            Debug.DrawLine(playerCamera.transform.position, raycastHit.point, Color.red, 5f);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, raycastHit.point);
            Debug.Log("HIT OBJ: " + raycastHit.collider);
        }
        currentMag--;
        //Debug.Log("Mag: " + currentMag + " / " + capacity + " From total" + currentAmmo);
        yield return new WaitForSeconds(fireDelay);
        isWaiting = false;
    }

    private bool CanShoot()
    {
        
        if (currentMag < 1 || isWaiting)
        {
            return false;
        }
        return true;
        
    }
    /// <summary>
    /// Checks ammo and starts reload routine.
    /// </summary>
    public void Reload()
    {
        if ((currentMag == capacity) || currentAmmo < 1)
        {
            Debug.Log("CANT RELOAD");
            return;
        }
        StartCoroutine(WaitForReload());

    }

    /// <summary>
    /// Reload coroutine which waits until reload time and animation are over before updating magazine and ammo reserve.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForReload()
    {
        Debug.Log("RELOADING");
        isWaiting = true;
        // Play anim
        yield return new WaitForSeconds(reloadTimeSeconds);
        if (currentAmmo > capacity)
        {
            currentAmmo -= capacity;
            currentMag = capacity;
        }
        else
        {
            currentMag = capacity - currentAmmo;
            currentAmmo = 0;
        }
        currentMag = capacity;
        isWaiting = false;
        Debug.Log("RELOADING DONE");
    }

    /// <summary>
    /// /// Gets spread and returns direction of bullet
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateBulletDirection()
    {
        /// Gets spread percent based on movement from min 20% to max 100%
        float spreadPercent = Mathf.Clamp(CalculateSpreadFromMovement(), 0f, 0.8f) + MIN_DEFAULT_SPREAD;
        float horizontalSpread = Random.Range(-horizontalRecoil, horizontalRecoil) * spreadPercent;
        float verticalSpread = Random.Range(-verticalRecoil, verticalRecoil) * spreadPercent;
    
        return playerCamera.transform.forward + new Vector3(horizontalSpread, verticalSpread, 0f);
    }
    /// <summary>
    /// Calculates the player's movement speed percentage from max speed and returns percentage for spread multiplier.
    /// </summary>
    /// <returns></returns>
    private float CalculateSpreadFromMovement()
    {
        return playerController.velocity.magnitude / playerMaxSpeed;
    }

    /// <summary>
    /// Calculates the fire delay in seconds between shots based on the given RPM
    /// </summary>
    /// <param name="RPM"></param>
    /// <returns></returns>
    private float CalculateFireDelaySeconds(float RPM)
    {
        return (1 / (RPM / 60));
    }



    /// <summary>
    /// Calculates aim delay time based on weight of the weapon. 
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    private float CalculateAimDelaySeconds(float weight)
    {
        return (AIM_DELAY_MULTIPLIER * weight);
    }

}
