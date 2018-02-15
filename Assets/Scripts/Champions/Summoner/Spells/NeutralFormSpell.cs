using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralFormSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //load GameObject(SummonerNeutral)
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
