using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTargetSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //new pet target=>enemy on mouse raycast
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
