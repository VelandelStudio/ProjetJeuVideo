using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //apply damages on one target (short cooldown) and apply stacks
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
