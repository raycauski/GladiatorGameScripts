using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoFirearm : Firearm
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Fire()
    {
        Debug.Log("Firing");
    }
}
