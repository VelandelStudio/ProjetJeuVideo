using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpell : Spell {


    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
