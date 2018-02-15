using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnihilationSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //big BOUM !!!!!!!! => apply heavy damages on everything with HP in the area destroy GameObject pet then load summonerNeutral (or launch NeutralFormSpell)
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
