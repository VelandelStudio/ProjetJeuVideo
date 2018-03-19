using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShieldSpell : Spell
{
    private Camera _cameraPlayer; // variable that will contain the player camera 

    // private GameObject _summoner;
    // private ShieldDecreaseDamageStatus decreaseDamageStatus;

    protected override void Start()
    {
        _cameraPlayer = GetComponentInChildren<Camera>(); // get player camera object

        base.Start();
    }


    public override void LaunchSpell()
    
    {
        // restore hp and increase defense of the player for a limited time then explode and make damages to monster around the player
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            ApplyStatus(GetComponent<EnergyShieldSpell>().Status[1], transform);
            Debug.Log(" Dammage dealt to the player reduced by" + int.Parse(OtherValues[0]));  // % dammage reduction
            // ApplyStatus(GetComponent<EnergyShieldSpell>().Status[2], transform);
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }

    }
 
     
}
