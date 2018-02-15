using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArpoonSpell : Spell
{

    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            // apply force to target toward the player until reached then stun it
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
