using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflagrationSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            // make damages in an area and apply status "touched" on target hit (it burns !!!!!!)
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
