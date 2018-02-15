using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShieldSpell : Spell
{

    public override void LaunchSpell()
    {
        // restore hp and increase defense of the player for a limited time then explode and make damages to monster around the player
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
