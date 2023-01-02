using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Firearm : MonoBehaviour
{
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


    protected float aimTime;
    /// constants
    protected float fireDelay;


    protected const float AIM_DELAY_MULTIPLIER = 2f / 15f;

   

    // Start is called before the first frame update
    void Start()
    {
        fireDelay = CalculateFireDelaySeconds(fireRateRPM);
        Debug.Log("Fire delay = " + fireDelay + " seconds");
        aimTime = CalculateAimDelaySeconds(weaponWeight);
        Debug.Log("Aim delay seconds = " + aimTime + " seconds, using " + AIM_DELAY_MULTIPLIER + " seconds per pound");
    }

    public abstract void Fire();
 

    public void Reload()
    {
        if ((currentMag == capacity) || currentAmmo < 1)
        {
            return;
        }
    }


    private bool HasAmmo()
    {
        return currentMag > 0;
    }

    private void ApplyRecoil()
    {

    }



    private float CalculateFireDelaySeconds(float RPM)
    {
        return (1 / (RPM / 60));
    }

    private float CalculateAimDelaySeconds(float weight)
    {
        return (AIM_DELAY_MULTIPLIER * weight);
    }

}
